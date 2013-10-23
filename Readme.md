Facebook Controls for Xamarin.iOS and Xamarin.Android
=====================================================

Source Code Setup
-----------------

* Clone the repo for this project. A fork of the core library and the library for the controls for Windows clients is included inside the repo as submodules, so we tell clone to download that recursively as well, and then we checkout those in the branches where our changes are being done.

		git clone --branch dev --recurse-submodules https://github.com/pupitetris/facebook-xamarin-sdk.git
		cd facebook-xamarin-sdk/facebook-winclient-sdk
		git branch xamarin
		cd ../facebook-csharp-sdk
		git branch xamarin-gui


Building
--------

* Open <tt>FB/facebook-xamarin-sdk/Source/Facebook.Client-iOS.sln</tt>
* Build and run under the Debug profile.

That's it!

TO-DO
-----
Some of the most important items that are pending:

* We are still using a Web Client for initial authentication.
* Debugging.
	* If you login and out two or three times, an unexpected page comes up and the GUI ends up at a dead end.
* Documentation.
* All of the Android controls.

