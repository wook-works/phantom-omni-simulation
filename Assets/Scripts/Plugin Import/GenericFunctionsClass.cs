using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.Text;

public class GenericFunctionsClass : MonoBehaviour {

	
	/*************************************************************/
	// Variables
	/*************************************************************/
	
	//Haptic Properties
	private HapticProperties myHapticPropertiesScript;
	

	//Access to script SimpleShapeManipulation
	public HapticClassScript myHapticClassScript;

	
	//GetHapticWorkSpace Values
	private float[] myWSPosition = new float[3];
	private float[] myWSSize = new float[3];

    //GetHapticWorkSpace Values for Two Workspace
    private float[] myTwoWsPosition = new float[6];
    private float[] myTwoWsSize = new float[6];

	//GetProxyValues - for haptic proxy position and orientation
	private double[] myProxyPosition = new double[3];
	private double[] myProxyRight = new double[3];
	private double[] myProxyDirection = new double[3];
	private double[] myProxyTorque = new double[3];
	private double[] myProxyOrientation = new double[4];

    //GetProxyValues - for haptic proxy position and orientation for two haptic device
    private double[] myTwoProxyPosition = new double[6];
    private double[] myTwoProxyOrientation = new double[8];

	//Haptic Environment Effect
	private ConstantForceEffect myContantForceScript;
	private ViscosityEffect myViscosityScript;
	private SpringEffect mySpringScript;
	private FrictionEffect myFrictionScript;
	private VibrationMotor myVibrationMotorScript;
	private VibrationContact myVibrationContactScript;
	private TangentialForce myTangentialForceScript;
	
	/*************************************************************/
	


	// Use this for initialization
	void Awake () {

	}

	/******************************************************************************************************************************************************************/

	/*************************************************************/
	// Generic functionnalities
	/*************************************************************/

	/******************************************************************************************************************************************************************/
	//generic function that returns the current mode
	public void IndicateMode()
	{
		if(PluginImport.GetMode () == 0)
			myHapticClassScript.HapticMode = "Simple contact"; 
		else if(PluginImport.GetMode () == 1)
			myHapticClassScript.HapticMode = "Object Manipulation";
		else if(PluginImport.GetMode () == 2)
			myHapticClassScript.HapticMode = "Custom Effect";
		else if(PluginImport.GetMode () == 3)
			myHapticClassScript.HapticMode = "Puncture";
	}
	

	/******************************************************************************************************************************************************************/

	//Haptic workspace generic functions
	public void SetHapticWorkSpace()
	{
		
		//Convert float3Array to IntPtr
		IntPtr dstPosPtr = ConverterClass.ConvertFloat3ToIntPtr(myHapticClassScript.myWorkSpacePosition);
		
		//Convert float3Array to IntPtr
		IntPtr dstSizePtr = ConverterClass.ConvertFloat3ToIntPtr(myHapticClassScript.myWorkSpaceSize);
		
		//Set Haptic Workspace for separate update
		//PluginImport.SetWorkspacePosition(dstPosPtr);
		//PluginImport.SetWorkspaceSize(dstSizePtr);
		
		//Set Haptic Workspace
		PluginImport.SetWorkspace(dstPosPtr,dstSizePtr);
	}

    //Haptic workspace generic functions for two haptic device
    //To set up two haptic workspaces, Workspace componenets (position and Size) 
    //must be store sequentially in the IntPtr which lenght will be set to 6 
    //(instead of 3 for a single workspace)
    public void SetTwoHapticWorkSpaces()
    {
        float[] myFloatPosition = new float[6];
        float[] myFloatSize = new float[6];

        //convert 2 Float3Array to Float6Array
        myFloatPosition = ConverterClass.Convert2Float3ToFloat6(myHapticClassScript.myWorkSpacePosition, myHapticClassScript.mySecondWorkSpacePosition);
        myFloatSize = ConverterClass.Convert2Float3ToFloat6(myHapticClassScript.myWorkSpaceSize, myHapticClassScript.mySecondWorkSpaceSize);

        //Convert float6 to IntPtr
        IntPtr dstPosPtr = ConverterClass.ConvertFloat6ToIntPtr(myFloatPosition);
        IntPtr dstSizePtr = ConverterClass.ConvertFloat6ToIntPtr(myFloatSize);

        //Set Haptic Workspace for separate update for two haptic devices
        //PluginImport.SetWorkspacePosition(dstPosPtr);
        //PluginImport.SetWorkspaceSize(dstSizePtr);

        //Set the workspace
        PluginImport.SetWorkspace(dstPosPtr, dstSizePtr);
    }
   
    /******************************************************************************************************************************************************************/
	
    public void GetHapticWorkSpace()
	{
		//Convert IntPtr to float3Array
		myWSPosition = ConverterClass.ConvertIntPtrToFloat3(PluginImport.GetWorkspacePosition());
		
		//Convert IntPtr to float3Array
		myWSSize = ConverterClass.ConvertIntPtrToFloat3(PluginImport.GetWorkspaceSize());
		
		//Refine my workspaceSize in the Unity Editor in case it has been changed
		myHapticClassScript.myWorkSpacePosition = ConverterClass.AssignFloat3ToFloat3(myWSPosition);
		
		//Refine my workspaceSize in the Unity Editor in case it has been changed
		myHapticClassScript.myWorkSpaceSize = ConverterClass.AssignFloat3ToFloat3(myWSSize);
	}

    //Get two haptic workspaces for two haptic device
    public void GetTwoHapticWorkSpaces()
    {
        //Convert IntPtr to float6Array
        myTwoWsPosition = ConverterClass.ConvertIntPtrToFloat6(PluginImport.GetWorkspacePosition());

        //Convert IntPtr to float6Array
        myTwoWsSize = ConverterClass.ConvertIntPtrToFloat6(PluginImport.GetWorkspaceSize());

        //Refine my workspaceSize in the Unity Editor in case it has been changed
        myHapticClassScript.myWorkSpacePosition = ConverterClass.SelectHalfFloat6toFloat3(myTwoWsPosition, 1);//Workspace position for device1
        myHapticClassScript.mySecondWorkSpacePosition = ConverterClass.SelectHalfFloat6toFloat3(myTwoWsPosition,2);//Workspace position for device2

        //Refine my workspaceSize in the Unity Editor in case it has been changed
        myHapticClassScript.myWorkSpaceSize = ConverterClass.SelectHalfFloat6toFloat3(myTwoWsSize, 1);//Workspace size for device1
        myHapticClassScript.mySecondWorkSpaceSize = ConverterClass.SelectHalfFloat6toFloat3(myTwoWsSize,2);//Workspace size for device2
    }

    /******************************************************************************************************************************************************************/



    /******************************************************************************************************************************************************************/

	public void UpdateGraphicalWorkspace()
	{
		//Position
		Vector3 pos;
		pos = ConverterClass.ConvertFloat3ToVector3(myWSPosition);
		myHapticClassScript.workSpaceObj.transform.position = pos;
		
		//Orientation
		myHapticClassScript.workSpaceObj.transform.rotation = Quaternion.Euler(0.0f,myHapticClassScript.myHapticCamera.transform.eulerAngles.y, 0.0f);
		
		//Scale
		Vector3 size;
		size = ConverterClass.ConvertFloat3ToVector3(myWSSize);
		myHapticClassScript.workSpaceObj.transform.localScale = size;
	}

    public void UpdateTwoGraphicalWorkspaces()
    {
        //Device 1
        //Position
        Vector3 pos1;
        pos1 = ConverterClass.ConvertFloat3ToVector3(myHapticClassScript.myWorkSpacePosition);
        myHapticClassScript.workSpaceObj.transform.position = pos1;

        //Orientation
        myHapticClassScript.workSpaceObj.transform.rotation = Quaternion.Euler(0.0f, myHapticClassScript.myHapticCamera.transform.eulerAngles.y, 0.0f);

        //Scale
        Vector3 size1;
        size1 = ConverterClass.ConvertFloat3ToVector3(myHapticClassScript.myWorkSpaceSize);
        myHapticClassScript.workSpaceObj.transform.localScale = size1;

        //Device 2
        //Position
        Vector3 pos2;
        pos2 = ConverterClass.ConvertFloat3ToVector3(myHapticClassScript.mySecondWorkSpacePosition);
        myHapticClassScript.secondWorkSpaceObj.transform.position = pos2;

        //Orientation
        myHapticClassScript.secondWorkSpaceObj.transform.rotation = Quaternion.Euler(0.0f, myHapticClassScript.mySecondHapticCamera.transform.eulerAngles.y, 0.0f);

        //Scale
        Vector3 size2;
        size2 = ConverterClass.ConvertFloat3ToVector3(myHapticClassScript.mySecondWorkSpaceSize);
        myHapticClassScript.secondWorkSpaceObj.transform.localScale = size2;
    }


	/******************************************************************************************************************************************************************/

	//Get Proxy Position and Orientation generic function
	public 	void GetProxyValues()
	{
		/*Proxy Position*/
		
		//Convert IntPtr to Double3Array
		myProxyPosition = ConverterClass.ConvertIntPtrToDouble3(PluginImport.GetProxyPosition());
		
		//Attach the Cursor Node
		Vector3 positionCursor = new Vector3();
		positionCursor = ConverterClass.ConvertDouble3ToVector3(myProxyPosition);
		
		//Assign Haptic Values to Cursor
		myHapticClassScript.hapticCursor.transform.position = positionCursor;
		
		
		//Proxy Right - Not use in that case
		//Convert IntPtr to Double3Array
		/*myProxyRight =  ConverterClass.ConvertIntPtrToDouble3(PluginImport.GetProxyRight());
		//Attach the Cursor Node
		Vector3 rightCursor = new Vector3();
		rightCursor = ConverterClass.ConvertDouble3ToVector3(myProxyRight);

		//Proxy Direction
		//Convert IntPtr to Double3Array
		myProxyDirection =  ConverterClass.ConvertIntPtrToDouble3( PluginImport.GetProxyDirection());
		//Attach the Cursor Node
		Vector3 directionCursor = new Vector3();
		directionCursor = ConverterClass.ConvertDouble3ToVector3(myProxyDirection);

		//Proxy Torque
		myProxyTorque = ConverterClass.ConvertIntPtrToDouble3(PluginImport.GetProxyTorque());
		//Attach the Cursor Node
		Vector3 torqueCursor = new Vector3();
		torqueCursor = ConverterClass.ConvertDouble3ToVector3(myProxyTorque);

		//Set Orientation
		myHapticClassScript.hapticCursor.transform.rotation = Quaternion.LookRotation(directionCursor,torqueCursor);*/
		
		//Proxy Orientation
		//Convert IntPtr to Double4Array
		myProxyOrientation = ConverterClass.ConvertIntPtrToDouble4(PluginImport.GetProxyOrientation());
		
		//Attach the Cursor Node
		Vector4 OrientationCursor = new Vector4();
		OrientationCursor = ConverterClass.ConvertDouble4ToVector4(myProxyOrientation);
		
		//Assign Haptic Values to Cursor
		myHapticClassScript.hapticCursor.transform.rotation =  new Quaternion(OrientationCursor.x,OrientationCursor.y,OrientationCursor.z,OrientationCursor.w);
	}

    //Get Proxy Position and Orientation generic function for two haptic devices
    public void GetTwoProxyValues()
    {
        /*Two Proxy Position*/

        //Convert IntPtr to Double3Array
        //myTwoProxyPosition = ConverterClass.ConvertIntPtrToDouble6(PluginImport.GetDevicePosition());
        myTwoProxyPosition = ConverterClass.ConvertIntPtrToDouble6(PluginImport.GetProxyPosition());

        //Attach the Cursor Node
        Vector3 positionCursor1 = new Vector3();
        Vector3 positionCursor2 = new Vector3();

        //Assign value to position vector
        positionCursor1 = ConverterClass.ConvertDouble3ToVector3(ConverterClass.SelectHalfdouble6toDouble3(myTwoProxyPosition,1));
        positionCursor2 = ConverterClass.ConvertDouble3ToVector3(ConverterClass.SelectHalfdouble6toDouble3(myTwoProxyPosition,2));

        //Assign Haptic Values to Cursor
        myHapticClassScript.hapticCursor.transform.position = positionCursor1;

        //Assign Haptic Values to Cursor
        myHapticClassScript.secondHapticCursor.transform.position = positionCursor2;



        //Proxy Orientation
        //Convert IntPtr to Double4Array
        myTwoProxyOrientation = ConverterClass.ConvertIntPtrToDouble8(PluginImport.GetProxyOrientation());

        //Attach the Cursor Node
        Vector4 OrientationCursor1 = new Vector4();
        Vector4 OrientationCursor2 = new Vector4();

        //Assign value to orientation vector
        OrientationCursor1 = ConverterClass.ConvertDouble4ToVector4(ConverterClass.SelectHalfdouble8toDouble4(myTwoProxyOrientation,1));
        OrientationCursor2 = ConverterClass.ConvertDouble4ToVector4(ConverterClass.SelectHalfdouble8toDouble4(myTwoProxyOrientation,2));

        //Assign Haptic Values to Cursor
        myHapticClassScript.hapticCursor.transform.rotation = new Quaternion(OrientationCursor1.x, OrientationCursor1.y, OrientationCursor1.z, OrientationCursor1.w);

        //Assign Haptic Values to Cursor
        myHapticClassScript.secondHapticCursor.transform.rotation = new Quaternion(OrientationCursor2.x, OrientationCursor2.y, OrientationCursor2.z, OrientationCursor2.w);

 
    }


    /******************************************************************************************************************************************************************/
	
	private int clickCount = 0;
	private GameObject manipObj = null;
	private Transform prevParent;
	
	public void GetTouchedObject()
	{
        //Convert Convert IntPtr To byte[] to String
        //string myObjStringName = ConverterClass.ConvertIntPtrToByteToString(PluginImport.GetTouchedObjectName());//PluginImport.GetTouchedObjectName() - To be deprecated
        string myObjStringName = ConverterClass.ConvertIntPtrToByteToString(PluginImport.GetTouchedObjName(1));
        Debug.Log ("The touched object is " + myObjStringName.ToString());
		
		//If in Manipulation Mode enable the manipulation of the selected object
		if(PluginImport.GetMode() == 1)
		{
			if(PluginImport.GetButton1State())
			{
				if(clickCount == 0)
				{
					//Set the manipulated object at first click
					manipObj = GameObject.Find (myObjStringName);
					
					//Setup Manipulated object Hierarchy as a child of haptic cursor - Only if object is declared as Manipulable object
					if(manipObj != null && !PluginImport.IsFixed(PluginImport.GetManipulatedObjectId()))
					{
							//Store the Previous parent object	
							prevParent = manipObj.transform.parent;
		
							//Asign New Parent - the tip of the manipulation object device
							manipObj.transform.parent = myHapticClassScript.hapticCursor.transform;
					}
						
				}
				clickCount++;
			}
			else 
			{
				//Reset Click counter
				clickCount = 0;
				
				//Reset Manipulated Object Hierarchy
				if (manipObj != null)
					manipObj.transform.parent = prevParent;
				
				//Reset Manipulated Object
				manipObj = null;

				//Reset prevParent
				prevParent = null;
			}

			//Only in Manipulation otherwise object are not moving so there is no need to proceed
			UpdateHapticObjectMatrixTransform();
		}
	}	

	/******************************************************************************************************************************************************************/

	public void SetHapticGeometry()
	{
		//Get array of all object with tag "Touchable"
		GameObject[] myObjects = GameObject.FindGameObjectsWithTag("Touchable") as GameObject[];
		
		for (int ObjId = 0; ObjId < myObjects.Length; ObjId++)
		{
			/***************************************************************/
			//Set the Transformation Matric of the Object
			/***************************************************************/
			//Get the Transformation matrix from object
			Matrix4x4 m = new Matrix4x4();

			//Build a transform Matrix from the translation/rotation and Scale parameters fo the object - Local Matrix
			//m.SetTRS(myObjects[ObjId].transform.position,myObjects[ObjId].transform.rotation,myObjects[ObjId].transform.localScale);

			//Build a transform Matrix from the translation/rotation and Scale parameters fo the object - Glabal Matrix
			m  = myObjects[ObjId].transform.localToWorldMatrix;
			
			//Convert Matrix4x4 to double16
			double[] matrix = ConverterClass.ConvertMatrix4x4ToDouble16(m);
			//Convert Double16 To IntPtr
			IntPtr dstDoublePtr = ConverterClass.ConvertDouble16ToIntPtr(matrix);
			
			//Convert String to Byte[] (char* in C++) and Byte[] to IntPtr
			IntPtr dstCharPtr = ConverterClass.ConvertStringToByteToIntPtr(myObjects[ObjId].name);
			
			//Send the transformation Matrix of the object
			PluginImport.SetObjectTransform(ObjId, dstCharPtr, dstDoublePtr);
			
			/***************************************************************/
			
			/***************************************************************/
			//Set the Mesh of the Object
			/***************************************************************/
			//Get Mesh of Object
			Mesh mesh = myObjects[ObjId].GetComponent<MeshFilter>().mesh;
			Vector3[] vertices = mesh.vertices;

			int[] triangles = mesh.triangles;
			
			//Reorganize the Array
			float[] verticesToSend = ConverterClass.ConvertVector3ArrayToFloatArray(vertices);
			//Allocate Memory according to needed space for float* (3*4)
			IntPtr dstVerticesArrayPtr = Marshal.AllocCoTaskMem(vertices.Length * 3 * Marshal.SizeOf(typeof(float)));
			//Copy to dstPtr
			Marshal.Copy(verticesToSend,0,dstVerticesArrayPtr,vertices.Length * 3);
			
			//Convert Int[] to IntPtr
			IntPtr dstTrianglesArrayPtr = ConverterClass.ConvertIntArrayToIntPtr(triangles);
			
			//Send the Raw Mesh of the object - transformation are not applied on the Mesh vertices
			PluginImport.SetObjectMesh(ObjId,dstVerticesArrayPtr, dstTrianglesArrayPtr,vertices.Length,triangles.Length);
			/***************************************************************/
			
			/***************************************************************/
			//Get the haptic parameter configuration
			/***************************************************************/
			ReadHapticProperties(ObjId, myObjects[ObjId]);
			/***************************************************************/
		}
	}

	//Haptic Properties generic function
	private void ReadHapticProperties(int ObjId, GameObject obj)
	{
		myHapticPropertiesScript = obj.transform.GetComponent<HapticProperties>();
		
		if (myHapticPropertiesScript == null)//Set default Values
		{
			PluginImport.SetStiffness(ObjId, 1.0f);
			PluginImport.SetDamping(ObjId, 0.0f);
			PluginImport.SetStaticFriction(ObjId, 0.0f);
			PluginImport.SetDynamicFriction(ObjId, 0.0f);
			PluginImport.SetTangentialStiffness(ObjId, 0.0f);
			PluginImport.SetTangentialDamping(ObjId, 0.0f);
			PluginImport.SetPopThrough(ObjId, 0.0f);
			PluginImport.SetPuncturedStaticFriction(ObjId, 0.0f);
			PluginImport.SetPuncturedDynamicFriction(ObjId, 0.0f);
			PluginImport.SetMass(ObjId,0.0f);
			PluginImport.SetFixed(ObjId,true);
			Debug.Log ("Haptic Characteristics not set for " + obj.name);
		}
		else
		{
			PluginImport.SetHapticProperty(ObjId,ConverterClass.ConvertStringToByteToIntPtr("stiffness"),myHapticPropertiesScript.stiffness);
			PluginImport.SetHapticProperty(ObjId,ConverterClass.ConvertStringToByteToIntPtr("damping"),myHapticPropertiesScript.damping);
			PluginImport.SetHapticProperty(ObjId,ConverterClass.ConvertStringToByteToIntPtr("staticFriction"),myHapticPropertiesScript.staticFriction);
			PluginImport.SetHapticProperty(ObjId,ConverterClass.ConvertStringToByteToIntPtr("dynamicFriction"),myHapticPropertiesScript.dynamicFriction);
			PluginImport.SetHapticProperty(ObjId,ConverterClass.ConvertStringToByteToIntPtr("tangentialStiffness"),myHapticPropertiesScript.tangentialStiffness);
			PluginImport.SetHapticProperty(ObjId,ConverterClass.ConvertStringToByteToIntPtr("tangentialDamping"),myHapticPropertiesScript.tangentialDamping);
			PluginImport.SetHapticProperty(ObjId,ConverterClass.ConvertStringToByteToIntPtr("popThrough"),myHapticPropertiesScript.popThrough);
			PluginImport.SetHapticProperty(ObjId,ConverterClass.ConvertStringToByteToIntPtr("puncturedStaticFriction"),myHapticPropertiesScript.puncturedStaticFriction);
			PluginImport.SetHapticProperty(ObjId,ConverterClass.ConvertStringToByteToIntPtr("puncturedDynamicFriction"),myHapticPropertiesScript.puncturedDynamicFriction);
			PluginImport.SetHapticProperty(ObjId,ConverterClass.ConvertStringToByteToIntPtr("mass"),myHapticPropertiesScript.mass);
			PluginImport.SetHapticProperty(ObjId,ConverterClass.ConvertStringToByteToIntPtr("fixed"),System.Convert.ToInt32(myHapticPropertiesScript.fixedObj));

			/*PluginImport.SetStiffness(ObjId, myHapticPropertiesScript.stiffness);
			PluginImport.SetDamping(ObjId, myHapticPropertiesScript.damping);
			PluginImport.SetStaticFriction(ObjId, myHapticPropertiesScript.staticFriction);
			PluginImport.SetDynamicFriction(ObjId, myHapticPropertiesScript.dynamicFriction);
			PluginImport.SetTangentialStiffness(ObjId, myHapticPropertiesScript.tangentialStiffness);
			PluginImport.SetTangentialDamping(ObjId, myHapticPropertiesScript.tangentialDamping);
			PluginImport.SetPopThrough(ObjId, myHapticPropertiesScript.popThrough);
			PluginImport.SetPuncturedStaticFriction(ObjId, myHapticPropertiesScript.puncturedStaticFriction);
			PluginImport.SetPuncturedDynamicFriction(ObjId, myHapticPropertiesScript.puncturedDynamicFriction);
			PluginImport.SetMass(ObjId,myHapticPropertiesScript.mass);
			PluginImport.SetFixed(ObjId,myHapticPropertiesScript.fixedObj);	*/
		}
	}

	public void UpdateHapticObjectMatrixTransform()
	{
		//Get array of all object with tag "Touchable"
		GameObject[] myObjects = GameObject.FindGameObjectsWithTag("Touchable") as GameObject[];
		
		for (int ObjId = 0; ObjId < myObjects.Length; ObjId++)
		{
			/***************************************************************/
			//Set the Transformation Matric of the Object
			/***************************************************************/
			//Get the Transformation matrix from object
			Matrix4x4 m = new Matrix4x4();
			//Build a transform Matrix from the translation/rotation and Scale parameters fo the object
			m.SetTRS(myObjects[ObjId].transform.position,myObjects[ObjId].transform.rotation,myObjects[ObjId].transform.localScale);
			
			//Convert Matrix4x4 to double16
			double[] matrix = ConverterClass.ConvertMatrix4x4ToDouble16(m);
			//Convert Double16 To IntPtr
			IntPtr dstDoublePtr = ConverterClass.ConvertDouble16ToIntPtr(matrix);
			
			//Convert String to Byte[] (char* in C++) and Byte[] to IntPtr
			IntPtr dstCharPtr = ConverterClass.ConvertStringToByteToIntPtr(myObjects[ObjId].name);
			
			//Send the transformation Matrix of the object
			PluginImport.SetObjectTransform(ObjId, dstCharPtr, dstDoublePtr);
			
			/***************************************************************/
		}
	}

	/******************************************************************************************************************************************************************/

	//Haptic Effects generic functions
	public void SetEnvironmentViscosity ()
	{
		//Get data from script
		myViscosityScript = transform.GetComponent<ViscosityEffect>();
		
		
		/*****************************
		* Viscous Force Example
		*****************************/
		//convert String to IntPtr
		IntPtr type = ConverterClass.ConvertStringToByteToIntPtr(myViscosityScript.Type);
		//Convert float[3] to intptr
		IntPtr position = ConverterClass.ConvertFloat3ToIntPtr(myViscosityScript.positionEffect);
		//Convert float[3] to intptr
		IntPtr direction = ConverterClass.ConvertFloat3ToIntPtr(myViscosityScript.directionEffect);
		
		//Set the effect
		PluginImport.SetEffect(type,myViscosityScript.effect_index, myViscosityScript.gain, myViscosityScript.magnitude, myViscosityScript.duration, myViscosityScript.frequency, position, direction);
		PluginImport.StartEffect(myViscosityScript.effect_index);
	}
	
	public void SetEnvironmentConstantForce()
	{
		
		myContantForceScript = transform.GetComponent<ConstantForceEffect>();
		
		/*****************************
		* Constant Force Example
		*****************************/
		//convert String to IntPtr
		IntPtr type = ConverterClass.ConvertStringToByteToIntPtr(myContantForceScript.Type);
		//Convert float[3] to intptr
		IntPtr position = ConverterClass.ConvertFloat3ToIntPtr(myContantForceScript.positionEffect);
		//Convert float[3] to intptr
		IntPtr direction = ConverterClass.ConvertFloat3ToIntPtr(myContantForceScript.directionEffect);
		
		//Set the effect
		PluginImport.SetEffect(type,myContantForceScript.effect_index, myContantForceScript.gain, myContantForceScript.magnitude, myContantForceScript.duration, myContantForceScript.frequency, position, direction);
		PluginImport.StartEffect(myContantForceScript.effect_index);
	}
	
	public void SetEnvironmentFriction()
	{
		myFrictionScript = transform.GetComponent<FrictionEffect>();
		
		/*****************************
		* Friction Force Example
 		*****************************/
		//convert String to IntPtr
		IntPtr type = ConverterClass.ConvertStringToByteToIntPtr(myFrictionScript.Type);
		//Convert float[3] to intptr
		IntPtr position = ConverterClass.ConvertFloat3ToIntPtr(myFrictionScript.positionEffect);
		//Convert float[3] to intptr
		IntPtr direction = ConverterClass.ConvertFloat3ToIntPtr(myFrictionScript.directionEffect);
		
		//Set the effect
		PluginImport.SetEffect(type,myFrictionScript.effect_index, myFrictionScript.gain, myFrictionScript.magnitude, myFrictionScript.duration, myFrictionScript.frequency, position, direction);
		PluginImport.StartEffect(myFrictionScript.effect_index);
	}
	
	public void SetEnvironmentSpring()
	{
		mySpringScript = transform.GetComponent<SpringEffect>();
		
		/*****************************
		* Spring Force Example
		*****************************/
		//convert String to IntPtr
		IntPtr type = ConverterClass.ConvertStringToByteToIntPtr(mySpringScript.Type);
		//Convert float[3] to intptr
		IntPtr position = ConverterClass.ConvertFloat3ToIntPtr(mySpringScript.positionEffect);
		//Convert float[3] to intptr
		IntPtr direction = ConverterClass.ConvertFloat3ToIntPtr(mySpringScript.directionEffect);
		
		//Set the effect
		PluginImport.SetEffect(type,mySpringScript.effect_index, mySpringScript.gain, mySpringScript.magnitude, mySpringScript.duration, mySpringScript.frequency, position, direction);
		PluginImport.StartEffect(mySpringScript.effect_index);
	}

	/******************************************************************************************************************************************************************/

	public void SetVibrationMotor()
	{
		myVibrationMotorScript = transform.GetComponent<VibrationMotor>();
		
		/*****************************
		* Vibration Motor Force Example
		*****************************/
		//convert String to IntPtr
		IntPtr type = ConverterClass.ConvertStringToByteToIntPtr(myVibrationMotorScript.Type);
		//Convert float[3] to intptr
		IntPtr position = ConverterClass.ConvertFloat3ToIntPtr(myVibrationMotorScript.positionEffect);
		//Convert float[3] to intptr
		IntPtr direction = ConverterClass.ConvertFloat3ToIntPtr(myVibrationMotorScript.directionEffect);
		
		//Set the effect
		PluginImport.SetEffect(type,myVibrationMotorScript.effect_index, myVibrationMotorScript.gain, myVibrationMotorScript.magnitude, myVibrationMotorScript.duration, myVibrationMotorScript.frequency, position, direction);
		PluginImport.StartEffect(myVibrationMotorScript.effect_index);
	}
	
	public void SetVibrationContact()
	{
		myVibrationContactScript = transform.GetComponent<VibrationContact>();
		
		/*****************************
		* Vibration Contact Force Example
		*****************************/
		//convert String to IntPtr
		IntPtr type = ConverterClass.ConvertStringToByteToIntPtr(myVibrationContactScript.Type);
		//Convert float[3] to intptr
		IntPtr position = ConverterClass.ConvertFloat3ToIntPtr(myVibrationContactScript.positionEffect);
		//Convert float[3] to intptr
		IntPtr direction = ConverterClass.ConvertFloat3ToIntPtr(myVibrationContactScript.directionEffect);
		
		//Set the effect
		PluginImport.SetEffect(type,myVibrationContactScript.effect_index, myVibrationContactScript.gain, myVibrationContactScript.magnitude, myVibrationContactScript.duration, myVibrationContactScript.frequency, position, direction);
		PluginImport.StartEffect(myVibrationContactScript.effect_index);
	}
	
	public void SetTangentialForce()
	{
		myTangentialForceScript = transform.GetComponent<TangentialForce>();
		
		/*****************************
		* Tangential Force Example
		*****************************/
		//convert String to IntPtr
		IntPtr type = ConverterClass.ConvertStringToByteToIntPtr(myTangentialForceScript.Type);
		//Convert float[3] to intptr
		IntPtr position = ConverterClass.ConvertFloat3ToIntPtr(myTangentialForceScript.positionEffect);
		//Convert float[3] to intptr
		IntPtr direction = ConverterClass.ConvertFloat3ToIntPtr(myTangentialForceScript.directionEffect);
		
		//Set the effect
		PluginImport.SetEffect(type,myTangentialForceScript.effect_index, myTangentialForceScript.gain, myTangentialForceScript.magnitude, myTangentialForceScript.duration, myTangentialForceScript.frequency, position, direction);
		PluginImport.StartEffect(myTangentialForceScript.effect_index);
	}
	/******************************************************************************************************************************************************************/
}
