//adapted from Greg Borenstein code in Making Things See, tracks closest point.  
//adaptions by Anthony Rhys and James Winchester
//further adaptations by John Brieger
import oscP5.*;
import netP5.*;
import SimpleOpenNI.*;


//******************************************
// Init
//******************************************
boolean use3Avg = false;
boolean useOutlierScreen = true;;
float jumpTolerance = 150;

OscP5 oscP5;
NetAddress myRemoteLocation;

SimpleOpenNI kinect;

int closestValue;
int closestX;
int closestY;

float[] lastVals;
int mod; // the modulus of the frame number
float lastX;
float lastY;

float lastMessageMillis;
float interval = 17;

//1525 is 5ft
int threshold = 1525;

void setup()
{
  size(640, 480);
  kinect = new SimpleOpenNI(this);
  kinect.enableDepth();
  background(0);
  
  oscP5 = new OscP5(this,12000);
  myRemoteLocation = new NetAddress("127.0.0.1",8338);
  
  lastVals = new float[6];
}

void draw()
{
  mod += 1;
  if (mod >= 3) mod = 0;
  
  closestValue = 8000;
  
  kinect.update();
  
  //background(0);
  // get the depth array from the kinect
  int[] depthValues = kinect.depthMap();
  
  // for each row in th depth image
  for(int y = 0; y < 480; y++){
    for(int x = 0; x < 640; x++){
      
      //reverse x
      int reversedX = 640-x-1;
    
    
      // pull ut the corresponding value from the depth array
      int i = reversedX + y * 640;
      int currentDepthValue = depthValues[i];
      
      // if that pixel is the closest one we've seen so far and
      // is within a range 620 is 2 feet 1525 is 5 feet
      if(currentDepthValue > 310 && currentDepthValue < 1625
      && currentDepthValue < closestValue){
        
      
          //save its value
          closestValue = currentDepthValue;
          // and save its position (both x and y coordinates)
          closestX = x;
          closestY = y;
        }
      }
    }
    
    //************************************************
    // The xexy part
    //************************************************
    // draw the depth image on the screen or block it
    // image(kinect.depthImage(),0,0);
   fill(255);
     rect(0, 0, width, height);
     float interpolatedX, interpolatedY;
     if (useOutlierScreen && 
     sqrt(pow((lastX - closestX), 2) + pow((lastY - closestY), 2)) > jumpTolerance) {
       interpolatedX = lastX;
       interpolatedY = lastY;
     }
     else {
      interpolatedX = lerp(lastX, closestX, 0.3f);
      interpolatedY = lerp(lastY, closestY, 0.3f);
     }
      
    if(closestValue<threshold){
      fill(254);
      ellipse(interpolatedX, interpolatedY, 60, 60);
    }
    lastX = interpolatedX;
    lastY = interpolatedY;
  
  if(millis()-lastMessageMillis > interval ){
    lastMessageMillis = millis();
    if(closestValue<threshold){
     
     float msgX = 0;
     float msgY = 0;;
     
     if (use3Avg) {
       lastVals[2*mod] = interpolatedX;
       lastVals[2*mod + 1] = interpolatedY;
       for (int i = 0; i < 3; i++) {
         msgX += lastVals[2*i];
         msgY += lastVals[2*i + 1];
       }
       msgX /= 3.0f;
       msgY /= 3.0f;
     }
     
     else {
       msgX = interpolatedX;
       msgY = interpolatedY;
     } 
     
     OscMessage myMessage = new OscMessage("/staffPos");
       myMessage.add(msgX);
       myMessage.add(msgY);
       oscP5.send(myMessage, myRemoteLocation); 
       //println("Staff X:"+interpolatedX+" Y:"+interpolatedY);
    }
    else{
      /*OscMessage myMessage = new OscMessage("/threshold");
      oscP5.send(myMessage, myRemoteLocation);
      println("Threshold");*/
    }
  }
  }
  void mousePressed(){
    // clear screen
    background(0);
  }
