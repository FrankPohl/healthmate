# Health Mate
This is a contribution to the Azure hackathon on DEV. See this post for more details https://dev.to/devteam/hack-the-microsoft-azure-trial-on-dev-2ne5. 
## Why Health Mate
Health Mate was created with people in mind, that are not familiar with apps on the phone or on their desktop.
To help these people to collect important health information like blood pressure or pulse on a regular basis a natural language interface is used. 
What is measured with which value and when is captured in a dialog between the computer and the user.

## Using the app
After the app is started it is ready for input and prompts the user for it.
An example dialog may look likke that: 
```
The system welcomes the user and aks for his input     
    User says: My pulse was 72
System prompts to collect necessary values: When was it measured? 
    User says: Yesterday at 9:00 am
System aks: Should I save that?
    User says: Ok
System confirms save
```

## Technical Details
The app consists of two parts.
The server app that as a natural languagge processing app in LUIS and the client is a Xamarin.Forms app. 

### The LUIS App
There is not much shareable in the repository because the creation is not scriptable. But you can find the exported definitions (Intents, Sample Utterances and Entitites) [here](HealthMate/LUIS/LUISHealthMateEN.json) 


### The Client
The client is a Xamarin.Forms app. 

## Discussion of the solution
It turned out, that some programming is necessary to analze the results that the LUIS app returns to the client. In order to make the client act more naturally it can get pretty complex and it should be evaluated whether a bot developed with the Bot Framework makes this task easier. 

To help 

The goal of this app is to provide an app for people who cannot deal with computers very well.
Therefore it should be necessary to use a keyboard or a mouse to input the desired values.

There are other functions in the app that are more complicated but the main purpose, gather input with your just your voice is achived.

The app welcomes the user directly on the input and awaits speech input.

The app is implemented as a Xamarin.Forms app following the template Visual Studio provides.

In LUIS we have setup an app with different intents for the actions.
 
 The entities array was never filled therefore I used the JSon property with the same content.
 Therefore I had to install the Newtonsoft JSon nuget package
LUISHealthMateEN
Created the app in AZur

Intents defined for the different things.

My pulse was 73 at 9:30 am today.




## License
Everything in this repository is published under the MIT license.

