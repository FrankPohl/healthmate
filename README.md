# Health Mate
This is a contribution to the Azure hackathon on DEV. See this post for more details https://dev.to/devteam/hack-the-microsoft-azure-trial-on-dev-2ne5. 
## Why Health Mate
Health Mate was created with people in mind, that are not familiar with apps or used to use apps on the phone or desktop.
To help these people collect important health information like blood pressure or pulse on a regular basis Health Mate uses a speech as the interface instead of a keyboard. 
What is measured with which value and when is captured in a natural language dialog between the computer and the user.


## Using the app
The app can collect data for Temperature, Blood Pressure, Pulse and Glucose.
After the app is started it is ready for input and prompts the user to say what was measured.
An example dialog may look like that: 
```
The system welcomes the user and asks for his input     
    User says: My pulse was 72
System prompts to collect necessary values: When was it measured? 
    User says: Yesterday at 9:00 am
System asks: Should I save that?
    User says: Ok
System confirms save
```

## Technical Details
The app consists of two parts.
The server app that does the natural language processing. This is implemented as a LUIS app.
The second part is a Xamarin.Forms client app. This app implements the UI for the app.
Only the core of the client app, the input and LUIS communication part and processing part is implemented. The rest of the client app will not use any Azure services and is therefore not just mocked.

### The LUIS App
There is not much shareable in the repository because the creation is not scriptable. But you can find the exported definitions (Intents, Sample Utterances and Entities) [here](HealthMate/LUIS/LUISHealthMateEN.json)

For every parameter and action an intent is defined or one of the pre-defined intents is used.
For all the intents there are sample utterances with the marked entities defined that the system uses to train the model for the prediction. 

### The Client
The client is a Xamarin.Forms app. Starting point was the Visual Studio template for Xamarin.Forms app. 
The main part is the processing of the return values of the LUIS app to react appropriately.
In addition to the packages that are installed with the template the nuget package with the SDK for Cognitive Services and for Newtonsoft JSON must be installed. 
```
install-package Microsoft.CognitiveServices.Speech
install-package Newtonsoft.Json
```
The Cognitive services SDK connect the microphone stream to the LUIS app and returns the result of the natural language processing.
The JSON package was necessary to transfer some of the returned JSON into easy processable objects.  
To show that it works on desktop and mobile the Android version and the UWP app are tested and can be compiled with Visual Studio 2022.


## Discussion of the implementation
It turned out, that some programming is necessary to analyze the results that the LUIS app returns to the client. To make the client act more naturally this analysis can get pretty complex therefore it should be evaluated whether a bot developed with the Bot Framework will make this task easier. 
This is not a problem because I used Xamarin.Forms for the app development but will be a problem with every framework. Using a technology that offers SDKs for the Bot framework and the Cognitive Services is necessary to have the freedom of choice.  

## License
Everything in this repository is published under the MIT license which you can find [here](LICENSE).

