/***********************************************************************************
List of functions in Converter class
************************************************************************************
//Convert Double[3] to Vector3
public static Vector3 ConvertDouble3ToVector3(double[] value)

//Convert Double[4] to Vector4
public static Vector4 ConvertDouble4ToVector4(double[] value)

//Convert Float[3] to Vector3
public static Vector3 ConvertFloat3ToVector3(float[] value)

//Assign Float[3] to Other Float[3]
public static float[] AssignFloat3ToFloat3(float[] value) 

//Convert Vector3[] to Float[]
public static float[] ConvertVector3ArrayToFloatArray(Vector3[] myVectorArray) 

//Convert Matrix4x4 to double16
public static double[] ConvertMatrix4x4ToDouble16(Matrix4x4 m)

//Convert Double16 to IntPtr
public static IntPtr ConvertDouble16ToIntPtr(double[] double16)

//Convert byte (alias Char* in C++) to IntPtr
public static IntPtr ConvertStringToByteToIntPtr(string myString)

//Convert Convert IntPtr To byte[] to String
public static string ConvertIntPtrToByteToString(IntPtr myPtr)

//convert Int[] to IntPtr
public static IntPtr ConvertIntArrayToIntPtr(int[] myIntArray)

//Convert floatArray to IntPtr
public static IntPtr ConvertFloatArrayToIntPtr( float[] myFloatArray)
	
//Convert float3Array to IntPtr
public static IntPtr ConvertFloat3ToIntPtr(float[] myFloat)

//Convert IntPtr to float3Array
public static float[] ConvertIntPtrToFloat3(IntPtr myPtr)
 
//Convert IntPtr to float6Array
public static float[] ConvertIntPtrToFloat6(IntPtr myPtr)
 
//Convert float6Array to IntPtr
public static IntPtr ConvertFloat6ToIntPtr(float[] myFloat)

//Convert 2float3Array to 1 float6Array
public static float[] Convert2Float3ToFloat6(float[] myFloat1, float[] myFloat2)
 
//Convert IntPtr to Double3Array
public static double[] ConvertIntPtrToDouble3(IntPtr myPtr)

//Convert IntPtr to Double4Array
public static double[] ConvertIntPtrToDouble4(IntPtr myPtr) 
 
//Convert IntPtr to Double6Array
public static double[] ConvertIntPtrToDouble6(IntPtr myPtr)

//Convert IntPtr to Double8Array
public static double[] ConvertIntPtrToDouble8(IntPtr myPtr)

//Select half of float6Array and store it as Float3Array
public static float[] SelectHalfFloat6toFloat3(float[] myFloat, int whichHalf)

//Select half of Double6Array and store it as Double3Array
public static double[] SelectHalfdouble6toDouble3(double[] myDouble6, int whichHalf)

//Select half of Double8Array and store it as Double4Array
public static double[] SelectHalfdouble8toDouble4(double[] myDouble8, int whichHalf)

************************************************************************************/

using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.Text;

public class ConverterClass {

	//Convert Double[3] to Vector3
	public static Vector3 ConvertDouble3ToVector3(double[] value) 
	{
		Vector3 myVector3 = new Vector3();
		myVector3.x = (float)value[0];
		myVector3.y = (float)value[1];
		myVector3.z = (float)value[2];

		return myVector3;
	}

	//Convert Double[4] to Vector4
	public static Vector4 ConvertDouble4ToVector4(double[] value) 
	{
		Vector4 myVector4 = new Vector4();
		myVector4.w = (float)value[0];
		myVector4.x = (float)value[1];
		myVector4.y = (float)value[2];
		myVector4.z = (float)value[3];
		
		return myVector4;
	}

	//Convert Float[3] to Vector3
	public static Vector3 ConvertFloat3ToVector3(float[] value) 
	{
		Vector3 myVector3 = new Vector3();
		myVector3.x = value[0];
		myVector3.y = value[1];
		myVector3.z = value[2];
		
		return myVector3;
	}

	//Assign Float[3] to Other Float[3]
	public static float[] AssignFloat3ToFloat3(float[] value) 
	{
		float[] myFloat3 = new float[3];
		myFloat3[0] = value[0];
		myFloat3[1] = value[1];
		myFloat3[2] = value[2];
		
		return myFloat3;
	}

	//Convert Vector3[] to Float[]
	public static float[] ConvertVector3ArrayToFloatArray(Vector3[] myVectorArray) 
	{
		float[] floatArray = new float[myVectorArray.Length * 3];

		for (int i=0; i<myVectorArray.Length; i++)
		{
			floatArray[3*i] = myVectorArray[i].x;
			floatArray[3*i + 1] = myVectorArray[i].y;
			floatArray[3*i + 2] = myVectorArray[i].z;
		}

		return floatArray;
	}

	//Convert Matrix4x4 to double16
	public static double[] ConvertMatrix4x4ToDouble16(Matrix4x4 m) 
	{
		double[] double16 = new double[16];
		int factor = 0;

		for (int i = 0; i < 4; i++)
		{

			double16[factor+i] = (double)m.GetRow(i).x;
			double16[factor+i+1] = (double)m.GetRow(i).y;
			double16[factor+i+2] = (double)m.GetRow(i).z;
			double16[factor+i+3] = (double)m.GetRow(i).w;

			factor += 3;
		}
		
		return double16;
	}

	//Convert Double16 to IntPtr
	public static IntPtr ConvertDouble16ToIntPtr(double[] double16)
	{
		//Allocate Memory according to needed space for double16
		IntPtr dstDoublePtr = Marshal.AllocCoTaskMem(16 * Marshal.SizeOf(typeof(double)));
		//Copy to dstPtr
		Marshal.Copy(double16,0,dstDoublePtr,16);

		return dstDoublePtr;
	}

	//Convert byte (alias Char* in C++) to IntPtr
	public static IntPtr ConvertStringToByteToIntPtr(string myString)
	{
		//Assign to byte and put ending character "\0"
		byte[] myByte = Encoding.ASCII.GetBytes( myString + "\0");
		//Allocate Memory according to needed space for byte*
		IntPtr dstCharPtr = Marshal.AllocCoTaskMem(myByte.Length * Marshal.SizeOf(typeof(byte)));
		//Copy to dstPtr
		Marshal.Copy(myByte,0,dstCharPtr,myByte.Length);

		return dstCharPtr;
	}

	//Convert Convert IntPtr To byte[] to String
	public static string ConvertIntPtrToByteToString(IntPtr myPtr)
	{
		//Assigning to memory
		IntPtr srcPtr = myPtr;
		byte[] ByteDst = new byte[100];
		//Copy from srcPtr to Char*
		Marshal.Copy(srcPtr, ByteDst, 0, 100);
		
		//string myObjStringName = new string(objCharNameDst);
		string myString = Encoding.ASCII.GetString(ByteDst, 0, Array.IndexOf(ByteDst, (byte)0));

		return myString;
	}

	//convert Int[] to IntPtr
	public static IntPtr ConvertIntArrayToIntPtr(int[] myIntArray)
	{
		//Allocate Memory according to needed space 
		IntPtr dstIntArrayPtr = Marshal.AllocCoTaskMem(myIntArray.Length * Marshal.SizeOf(typeof(int)));
		//Copy to dstPtr
		Marshal.Copy(myIntArray,0,dstIntArrayPtr,myIntArray.Length);

		return dstIntArrayPtr;
	}

	//Convert floatArray to IntPtr
	public static IntPtr ConvertFloatArrayToIntPtr( float[] myFloatArray)
	{
		//Allocate Memory according to needed space for float* (3*4)
		IntPtr dstFloatArrayPtr = Marshal.AllocCoTaskMem(myFloatArray.Length * Marshal.SizeOf(typeof(float)));
		//Copy to dstPtr
		Marshal.Copy(myFloatArray,0,dstFloatArrayPtr,myFloatArray.Length);
		
		return dstFloatArrayPtr;
	}
	

	//Convert float3Array to IntPtr
	public static IntPtr ConvertFloat3ToIntPtr(float[] myFloat)
	{
		//Allocate Memory according to needed space for float* (3*4)
		IntPtr dstFloatPtr = Marshal.AllocCoTaskMem(3 * Marshal.SizeOf(typeof(float)));
		//Copy to dstPtr
		Marshal.Copy(myFloat,0,dstFloatPtr,3);

		return dstFloatPtr;
	}

	//Convert IntPtr to float3Array
	public static float[] ConvertIntPtrToFloat3(IntPtr myPtr)
	{
		float[] myFloat = new float[3];

		//Assigning to memory
		IntPtr srcPtr = myPtr;

		//Copy from srcPtr
		Marshal.Copy(srcPtr, myFloat, 0, 3);

		return myFloat;
	}

    //Convert IntPtr to float6Array
    public static float[] ConvertIntPtrToFloat6(IntPtr myPtr)
    {
        float[] myFloat = new float[6];

        //Assigning to memory
        IntPtr srcPtr = myPtr;

        //Copy from srcPtr
        Marshal.Copy(srcPtr, myFloat, 0, 6);

        return myFloat;
    }

    //Convert float6Array to IntPtr
    public static IntPtr ConvertFloat6ToIntPtr(float[] myFloat)
    {
        //Allocate Memory according to needed space for float* (3*4)
        IntPtr dstFloatPtr = Marshal.AllocCoTaskMem(6 * Marshal.SizeOf(typeof(float)));
        //Copy to dstPtr
        Marshal.Copy(myFloat, 0, dstFloatPtr, 6);

        return dstFloatPtr;
    }

    //convert 2 float3Array to a float6Array
    public static float[] Convert2Float3ToFloat6(float[] myFloat1, float[] myFloat2)
    {
        float[] myFloat = new float[6];

        myFloat[0] = myFloat1[0];
        myFloat[1] = myFloat1[1];
        myFloat[2] = myFloat1[2];
        myFloat[3] = myFloat2[0];
        myFloat[4] = myFloat2[1];
        myFloat[5] = myFloat2[2];

        return myFloat;

    }

	//Convert IntPtr to Double3Array
	public static double[] ConvertIntPtrToDouble3(IntPtr myPtr)
	{
		double[] myDouble = new double[3];
		
		//Assigning to memory
		IntPtr srcPtr = myPtr;
		
		//Copy from srcPtr
		Marshal.Copy(srcPtr, myDouble, 0, 3);
		
		return myDouble;
	}
	
	//Convert IntPtr to Double4Array
	public static double[] ConvertIntPtrToDouble4(IntPtr myPtr)
	{
		double[] myDouble = new double[4];
		
		//Assigning to memory
		IntPtr srcPtr = myPtr;
		
		//Copy from srcPtr
		Marshal.Copy(srcPtr, myDouble, 0, 4);
		
		return myDouble;
	}

    //Convert IntPtr to Double6Array
    public static double[] ConvertIntPtrToDouble6(IntPtr myPtr)
    {
        double[] myDouble = new double[6];

        //Assigning to memory
        IntPtr srcPtr = myPtr;

        //Copy from srcPtr
        Marshal.Copy(srcPtr, myDouble, 0, 6);

        return myDouble;
    }

    //Convert IntPtr to Double6Array
    public static double[] ConvertIntPtrToDouble8(IntPtr myPtr)
    {
        double[] myDouble = new double[8];

        //Assigning to memory
        IntPtr srcPtr = myPtr;

        //Copy from srcPtr
        Marshal.Copy(srcPtr, myDouble, 0, 8);

        return myDouble;
    }

    /***********************************************************************************/

    public static float[] SelectHalfFloat6toFloat3(float[] myFloat6, int whichHalf)
    {
        float[] myFloat3 = new float[3];

        Mathf.Clamp(whichHalf, 1, 2);

        if (whichHalf == 1)
        {
            //Assign the first half of the float6
            for (int i = 0; i < 3; i++)
                myFloat3[i] = myFloat6[i];
        }
        else
        {
            //Assign the second half of the float6
            for (int i = 0; i < 3; i++)
                myFloat3[i] = myFloat6[i + 3];
        }

        return myFloat3;
    }

    public static double[] SelectHalfdouble6toDouble3(double[] myDouble6, int whichHalf)
    {
        double[] myDouble3 = new double[3];

        Mathf.Clamp(whichHalf, 1, 2);

        if (whichHalf == 1)
        {
            //Assign the first half of the float6
            for (int i = 0; i < 3; i++)
                myDouble3[i] = myDouble6[i];
        }
        else
        {
            //Assign the second half of the float6
            for (int i = 0; i < 3; i++)
                myDouble3[i] = myDouble6[i + 3];
        }

        return myDouble3;
    }

    //Select half of Double8Array and store it as Double4Array
    public static double[] SelectHalfdouble8toDouble4(double[] myDouble8, int whichHalf)
    {
        double[] myDouble4 = new double[4];

        Mathf.Clamp(whichHalf, 1, 2);

        if (whichHalf == 1)
        {
            //Assign the first half of the float6
            for (int i = 0; i < 4; i++)
                myDouble4[i] = myDouble8[i];
        }
        else
        {
            //Assign the second half of the float6
            for (int i = 0; i < 4; i++)
                myDouble4[i] = myDouble8[i + 4];
        }

        return myDouble4;
    }
	
}
