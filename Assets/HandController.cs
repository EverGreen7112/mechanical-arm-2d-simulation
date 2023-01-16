using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HandController : MonoBehaviour
{
    private Vector2 destination;   //where the arm needs to go
    private Vector2 startPos = Vector2.zero; //where the hand begins
    private float arm1Length;//length of the arm 1
    private float arm2Length;//length of the arm 2
    private float height;     //height of the vector between startPos and destination 
    private float width;      //width of the vector between startPos and destination
    private float arm1Angle; //the angle the first arm in degrees
    private float arm2Angle; //the angle the second arm in degrees
    private float pointsVecLength; //the length of the vector of the points
    public InputField destinationX;
    public InputField destinationY;
    public InputField lengthArm2;
    public InputField lengthArm1;

    private bool drawLines = false;
    
    void Update(){
        if(drawLines)
            DrawLines();
    }

    public void updateArm(){
  
        destination = new Vector2(float.Parse(destinationX.text),float.Parse(destinationY.text));
        arm1Length = float.Parse(lengthArm1.text);
        arm2Length = float.Parse(lengthArm2.text);
                
        CalcAngles();
        if(Possible()){
            drawLines = true;
        }
    }

     //calc and update the angles of the 2 arms 
    void CalcAngles(){ 
        
        height = Mathf.Abs(startPos.y - destination.y);
        width = Mathf.Abs(startPos.x - destination.x);

        pointsVecLength = Mathf.Sqrt(height * height + width * width);

        float tempAngle = Mathf.Rad2Deg * Mathf.Atan(height/width);
        float tempAngle2 = CalcAngleLawOfCosines(arm2Length,arm1Length,pointsVecLength);
        arm1Angle = tempAngle + tempAngle2;
        //CalcAngleLawOfCosines(pointsVecLength,arm1Length,arm2Length);for the full angle
        arm2Angle = Mathf.Rad2Deg * Mathf.Asin(Mathf.Abs(CalcArmVector(startPos,arm1Angle,arm1Length).y - destination.y) / arm2Length);
    }
    //returns the angle var in the law of cosines (x) : a^2 = b^2 + c^2 - 2 * b * c * cos(x)
    float CalcAngleLawOfCosines(float a,float b,float c){
        return Mathf.Rad2Deg * Mathf.Acos((a * a - b * b - c * c) / (-2 * b * c)); 
    }
    //draws the hand of the robot
    void DrawLines(){
        Vector2 arm1Vector = CalcArmVector(startPos,arm1Angle,arm1Length);
        Vector2 arm2Vector = CalcArmVector(arm1Vector,arm2Angle,arm2Length);
        Debug.DrawLine(startPos,arm1Vector,Color.red); //draw arm1 
        Debug.DrawLine(arm1Vector,arm2Vector,Color.red); //draw arm 2
        
    }
    //calcs the vector of one of the arms
    Vector2 CalcArmVector(Vector2 start,float armAngle, float armLength){
        float y = Mathf.Sin(Mathf.Deg2Rad * armAngle) * armLength;
        float x = Mathf.Cos(Mathf.Deg2Rad * armAngle) * armLength;
        return new Vector2(x + start.x , y + start.y);
    }

    //is it possible to move the arm to destination
    bool Possible(){
        return Mathf.Abs(arm1Length) + Mathf.Abs(arm2Length) >= Mathf.Abs(pointsVecLength);
    }

}
