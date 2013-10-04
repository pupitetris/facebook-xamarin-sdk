Facebook Controls for Xamarin.iOS and Xamarin.Android
=====================================================

Source Code Setup
-----------------

This is the recommended layout so that the paths inside the projects work without modification.

* Create a directory that will be our source root. I'll name it FB.

		mkdir FB
		
* Clone our fork of Outercurve's Facebook SDK for .Net, using the xamarin-gui branch.

		git clone --branch xamarin-gui https://github.com/pupitetris/facebook-csharp-sdk.git
		
* Clone the repo for this project. A fork of the controls for Windows clients is included inside the repo as a submodule, so we tell clone to download that recursively as well.

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

* FriendPicker for iOS.
* We are still using a Web Client for initial authentication.
* Debugging.
	* If you login and out two or three times, an unexpected page comes up and the GUI ends up at a dead end.
* Performance.
	* async Image loading should be set up with a queue to limit the number of threads created.
* Documentation.
* All of the Android controls.

