VREdit
=====

I changed the following to make Fungus work with my VR application. Please find all the files and scripts that I had to change to get my application to work well.

MenuDialog, SayDialog prefabs need to be made world space and "InteractiveObject" script needs to be added on each button in case of menuDialog and directly on SayDialog in case of it. Also, an Event trigger will be needed to be added to both, respectively. 

Google VR SDK needs to be added to the project. Although you will find GVR files in the plugins folder, not all GVR SDK files are included. You will also find Speech to text jar file in the plugins folder, SpeechEngine class uses these files, so be sure to include it.

Be sure to add "SpeechEngine" on an empty game object in your scene. Speech Engine only works for Android though, as it uses built in Android libraries, for any other Speech recognition solutions, you will need to change this SpeechEngine class.





Fungus
======

The goal of Fungus is to provide a free, open source tool for creating interactive storytelling games in Unity 3D. Fungus is designed to be easy to learn for beginners to Unity 3D, especially for people with no coding experience. For power users, it provides an intuitive, fast workflow for visual scripting and interactive storytelling. Fungus is being used to create Visual Novels, Point and Click Adventure Games, Childrens Stories, Hidden Object Games, eLearning apps and also some frankly weird stuff which defies classification :)

- Author: Chris Gregan
- Website: fungusgames.com
- Email: chris@snozbot.com
- Twitter: @gofungus
- Facebook: facebook.com/fungusgames

Installation
============

Download & installation instructions and tutorial videos are available on the official Fungus website.
http://fungusgames.com

Support
=======

If you have questions about Fungus, please search our forum first as someone may have had the same issue already. If you can't find an answer please start a new discussion and we'll answer you as soon as we can. Fungus is designed for beginners and we love to hear from users so please don't be shy about posting!
http://fungusgames.com/forum

You can also join into our chat room.
[![Join the chat at https://gitter.im/snozbot/fungus](https://badges.gitter.im/snozbot/fungus.svg)](https://gitter.im/snozbot/fungus?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

Contributing
============

To contribute code to Fungus, please see [CONTRIBUTING][contributing]. If you are interested in contributing in some other way (art, audio, documentation, pizza) just email me at chris at snozbot.com.

[contributing]: https://github.com/snozbot/fungus/blob/master/CONTRIBUTING.md

Many thanks to everyone who has contributed code to the project.
https://github.com/snozbot/fungus/graphs/contributors

Building the documentation
==========================

1. Download and install Doxygen from http://www.doxygen.org
2. Run Doxygen and open Docs/Doxyfile
3. Switch to the Run tab and click Run Doxygen.
4. The documentation will be built in the Builds/Docs folder.

To contribute to the documentation please send in a pull request with the changes.

Running the automated tests
===========================

The Unity Test Tools contains a tool called the Platform Runner which builds and runs all the test scenes automatically.

There is currently an issue with the Unity Test Tools on OSX that prevents the Platform Runner running all tests in the project. You can open each scene in the Tests folder individually and use the Unity Test Runner window to run all tests in the editor.

On Windows:
1. Open the Platform Runner via Unity Test Tools > Platform Runner
2. Select the platforms and tests you wish to run (defaults to running all).
3. Run the tests and wait for the results. All tests should pass.


