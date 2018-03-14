# Phantom-Omni-Project
Haptic / Force-Feedback Interface with OMNI Phantom for Robot Guidance

Authors: Myron Smith, Philip Mullins, Ali Elsaadi, Quoc Nguyen

Introduction

This project presents a software application for medical intention. It consists
of a three dimensional virtual environment created in Unity, which are
graphical scenarios in which the user interacts in real time using sensory
devices for vision and sense of movement.
The virtual environment allows haptic devices such as the Phantom Omni, a
device with peripheral input and output that allow user to interact with our
virtual environment, to simulate the sensation corresponding to the touch for
which forces return.

Methodology

Equipment and software necessary: Phantom Omni Haptic Device, Unity,
C#, OpenGL
Phantom Omni interface acts as a bidirectional communication device
between the user and the object in the virtual environment. This interface
allows the user to navigate through a virtual tube (blood vessel), and
perceive reaction forces depending whether or not there is contact between
the haptic cursor (the pill) and the tube. The user must move an object
through a three-dimensional tube. When the object comes into contact with
one of the walls of the tube, the user perceives the corresponding collision
force through the haptic device.
The magnitude of the reaction force is proportional to the level of the surface
intact of the pill within the wall of the tube. The Phantom Omni device
contains a rotating base attached with some joints and has 6 degree of
freedom that allow user to indicate the position and orientation of the haptic
cursor within our virtual environment.
With the addition of OpenGL haptic plugin that connects the Phantom to the
computer, we’re able to interface the device to unity. Scripts written in C#
allows the user to manipulate objects, set restraints, boundaries, and force
feedback.


The following plug-in and scripts include:

A SimpleHapticPlugin.dll

-Developed at the School of Simulation and Visualization,
Glasgow School of Art, a library built in C++ providing limited
implementation of the OpenHaptics toolkit HDAPI (low level
haptic device access) and HLAPI (high level haptic rendering)
functions allowing haptic interaction in Unity 5.x Game Engine
simulations through Unity3D's C# front-end scripting language.


HapticProperties.cs

-Stores the haptic environment properties


GenericFunctionsClass.cs

-provides set/get methods to HapticProperties and
HapticClassScript variables/methods


PluginImport.cs

-file imports methods from the ASimpleHapticPlugin.dll,
making them available for use in Unity scripts


HapticClassScript.cs

-file provides Unity Objects to GenericFunctionClass


SimpleShapeManipulation.cs

-file provide defines haptic behavior experienced in Unity scene


Results

The proposed method was tested for haptic feedback when the pill comes in
contact with the outer walls of the artery.
-Complete workspace of artery

When starting the executable file, the expected result includes controlling
the pill in the free space with the haptic device.
Sample image of the pill moving in free space until it enters the
bounded tube.
Once inside the tube, the pill is enabled to traverse through it while being
restricted within the bounds providing haptic feedback to the Omni.

Sample image of the pill traversing through the tube
If there is visual contact between the pill and the artery boundaries, the
haptic device will provide force feedback in order to guide the pill back onto
it’s traversal path.

Sample image of contact with boundaries producing force
feedback.

Limitation

The plugin did not allow access to raw data from the haptic device.
The data provided limited the ability to integrate the separate
workspaces of the haptic and Unity dynamically.

YouTube link to demo: https://youtu.be/DoSwg3S898E

Conclusion

In this project, the Phantom Omni haptic device demonstrated force
feedback by interfacing along a Unity Scenario with descriptive components
developed by C# scripts. The proposed result aligned with the project’s
expectations by providing the correct feedback when the pill collided with the
artery tube. Thus, handling feedback by guiding the tube safely back to its traversal
path.
By allowing the haptic device to calculate the precise and accurate
measurements of the procedure, it increases the chances of a successful operation.
This demonstration supports the technological medical movement by providing
hands-off and minimal error procedures. With the advancement of robotic
technology, more tasks using haptic feedback are being performed by robots to
reduce execution time and minimize human mistakes, such as errors caused by
negligence and/or exhaustion.
