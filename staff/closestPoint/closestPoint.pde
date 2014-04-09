//adapted from Greg Borenstein code in Making Things See, tracks closest point.  
//adaptions by Anthony Rhys and James Winchester
//further adaptations by John Brieger
import oscP5.*;
import netP5.*;
import SimpleOpenNI.*;


//******************************************
// Init
//******************************************
boolean useOutlierScreen = false;
float jumpTolerance = 9;
int stabThreshold = 30;

boolean fire = false;
KPacket[] frames;
int mod;

OscP5 oscP5;
NetAddress myRemoteLocation;

SimpleOpenNI kinect;

int lastClosestValue;
int closestValue;
int closestX;
int closestY;

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
  
  frames = new KPacket[4];
}

private float distance(float x0, float y0, float x1, float y1) {
  return sqrt(((x0 - x1) * (x0 - x1)) + ((y0 - y1) * (y0 - y1)));
}

void draw()
{ 
  lastClosestValue = closestValue;
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
      if(currentDepthValue > 10 && currentDepthValue < 5625
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
    // The sexy part
    //************************************************
    // draw the depth image on the screen or block it
     //image(kinect.depthImage(),0,0);
     fill(255);
     rect(0, 0, width, height);
     float interpolatedX, interpolatedY;
     
     interpolatedX = lerp(lastX, closestX, 0.3f);
     interpolatedY = lerp(lastY, closestY, 0.3f);
      
    if(closestValue<threshold){
      float iX = interpolatedX;
      float iY = interpolatedY;
      KPacket f1 = frames[(mod + 1) % 4];
      KPacket f2 = frames[(mod + 2) % 4];
      KPacket f3 = frames[(mod + 3) % 4];
      boolean corrected = false;
      
      // 2-FRAME CORRECTION
      if (f1 != null && f3 != null &&
          distance(f1.x, f1.y, f3.x, f3.y) > jumpTolerance &&
          distance(f2.x, f2.y, f3.x, f3.y) > jumpTolerance &&
          distance(iX, iY, f3.x, f3.y) < jumpTolerance) {
            
            f2.x = iX;
            f2.y = iY;
            f2.closestPoint = (f3.closestPoint + closestValue / 2);
            f1.x = iX;
            f1.y = iY;
            f1.closestPoint = (f3.closestPoint + closestValue / 2);
            corrected = true;
      }
      
      // 1-FRAME CORRECTION
      if (frames[3] != null && 
          !corrected &&
          distance(f1.x, f1.y, f2.x, f2.y) > jumpTolerance && 
          distance(iX, iY, f2.x, f2.y) < jumpTolerance) {
            
            f1.x = iX;
            f1.y = iY;
            f1.closestPoint = (f2.closestPoint + closestValue) / 2;
      }
      corrected = false;
          
      frames[mod] = new KPacket(closestValue, interpolatedX, interpolatedY);
      
      fill(254);
      
      // STAB MOTION DETECTED
      if (lastClosestValue - closestValue > stabThreshold) {
        fill(#FF0000);
        fire = true;
      }
      if (f2 != null) ellipse(f2.x, f2.y, 30, 30);
      else ellipse(iX, iY, 30, 30);
    }
    
    lastX = interpolatedX;
    lastY = interpolatedY;
  
  if(millis()-lastMessageMillis > interval ){
    lastMessageMillis = millis();
    if(closestValue<threshold){
     
     KPacket f3 = frames[(mod + 3) % 4];
     float msgX = 0;
     float msgY = 0;
     
     if (f3 != null) {
       msgX = -1 * (f3.x - 320) / 320;
       msgY = -1 * (f3.y - 240) / 240;
     }
     
     OscMessage myMessage = new OscMessage("/staffPos");
       myMessage.add(msgX);
       myMessage.add(msgY);
       oscP5.send(myMessage, myRemoteLocation);
      
     if (fire) {
       OscMessage fireMessage = new OscMessage("/fire");
       oscP5.send(fireMessage, myRemoteLocation);
       fire = false;
     }
    }
    else{
      /*OscMessage myMessage = new OscMessage("/threshold");
      oscP5.send(myMessage, myRemoteLocation);
      println("Threshold");*/
    }
  }
  mod += 1;
  if (mod == 4) mod = 0;
  }
  void mousePressed(){
    // clear screen
    background(0);
  }
