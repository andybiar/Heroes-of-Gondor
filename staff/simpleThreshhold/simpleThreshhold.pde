//adapted from Greg Borenstein code in Making Things See, tracks closest point.  
//adaptions by Anthony Rhys and James Winchester
//further adaptations by John Brieger
import oscP5.*;
import netP5.*;
import SimpleOpenNI.*;


//******************************************
// Init
//******************************************
boolean inRange = false;
boolean fired = false;

OscP5 oscP5;
NetAddress myRemoteLocation;

SimpleOpenNI kinect;

int closestValue;

float lastMessageMillis;
float interval = 17;

//1525 is 5ft, so 305/foot?
int threshold = 610;

void setup()
{
  size(640, 480);
  kinect = new SimpleOpenNI(this);
  kinect.enableDepth();
  background(0);
  
  oscP5 = new OscP5(this,12000);
  myRemoteLocation = new NetAddress("127.0.0.1",8338);
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
      
      int reversedX = 640-x-1;
      // pull ut the corresponding value from the depth array
      int i = reversedX + y * 640;
      int currentDepthValue = depthValues[i];
      
      // if that pixel is the closest one we've seen so far and
      // is within a range 620 is 2 feet 1525 is 5 feet
      if(currentDepthValue > 50 && currentDepthValue < 5625
      && currentDepthValue < closestValue){
        
          //save its value
          closestValue = currentDepthValue;
        }
      }
    }
    
     image(kinect.depthImage(),0,0);
     fill(255);
     //rect(0, 0, width, height);
      
    if(closestValue<threshold){
      inRange = true;
      fill(254);
  
  if(millis()-lastMessageMillis > interval ){
    lastMessageMillis = millis();
  
     if (inRange && !fired) {
       fill(#ff0000);
       ellipse(40,40,40,40);
       
       OscMessage fireMessage = new OscMessage("/fire");
       oscP5.send(fireMessage, myRemoteLocation);
       fired = true;
     }
    }
  }
  else {
    inRange = false;
    fired = false;
  }
}
