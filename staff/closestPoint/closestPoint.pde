//adapted from Greg Borenstein code in Making Things See, tracks closest point.  
//adaptions by Anthony Rhys and James Winchester
//further adaptations by John Brieger
import oscP5.*;
import netP5.*;
import SimpleOpenNI.*;

OscP5 oscP5;
NetAddress myRemoteLocation;

SimpleOpenNI kinect;

int closestValue;
int closestX;
int closestY;

float lastX;
float lastY;

//declare global variables for previous
//x and y co ordinates
int previousX;
int previousY;

float lastMessageMillis;
float interval = 17;

//1525 is 5ft
int threshold = 1000;

void setup()
{
  size(640, 480);
  kinect = new SimpleOpenNI(this);
  kinect.enableDepth();
  background(0);
  
  oscP5 = new OscP5(this,12000);
  myRemoteLocation = new NetAddress("127.0.0.1",12000);
}

void draw()
{
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
    
    // draw the depth image on the screen or block it
   // image(kinect.depthImage(),0,0);
   fill(255);
     rect(0, 0, width, height);
        float interpolatedX = lerp(lastX, closestX, 0.3f);
    float interpolatedY = lerp(lastY, closestY, 0.3f);
    if(closestValue<threshold){
    fill(254);
    ellipse(interpolatedX, interpolatedY, 60, 60);
    }
    lastX = interpolatedX;
    lastY = interpolatedY;
  if(millis()-lastMessageMillis > interval ){
    lastMessageMillis = millis();
    if(closestValue<threshold){
     OscMessage myMessage = new OscMessage("/staffPos");
       myMessage.add(closestX);
       myMessage.add(closestY);
       oscP5.send(myMessage, myRemoteLocation); 
       println("Staff X:"+closestX+" Y:"+closestY);
    }
    else{
      OscMessage myMessage = new OscMessage("/threshold");
      oscP5.send(myMessage, myRemoteLocation);
      println("Threshold");
    }
  }
  }
  void mousePressed(){
    // clear screen
    background(0);
  }
