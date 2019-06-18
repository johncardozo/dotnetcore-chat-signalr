# A Simple Chat - SignalR - ASP.NET MVC Example

## Prerequisites
- Visual Studio Code
- .Net Core 2.2
- NodeJS

# Developing steps
The steps to create this project was the following:

## 1. Create a project
Create a project named `signatr-chat`:

    dotnet new mvc --name signalr-chat

## 2. Add SignalR package
In the project folder, add SignalR package to the project:

    dotnet add package Microsoft.AspNet.SignalR

To check the package was installed correctly:

    dotnet list package

# 3. Install SignalR javascript client

Execute the following commands in project root folder:

    npm init y
    npm install @aspnet/signalr

Create the folder `wwwroot\lib\signalr` and copy the file `signalr.js` from 

    node_modules\@aspnet\signalr\dist\browser\
to

    wwwroot\lib\signalr


## 4. Create a SignalR Hub class
Check the class `Hubs\Chat.cs` in the source code

## 5. Update Startup.cs registering SignalR services

```csharp
// Import Hub namespace
using signalr_chat.Hubs;

// Update ConfigureServices method
services.AddSignalR();

// Update Configure method
app.UseSignalR (builder =>
{
    builder.MapHub<Chat>("/chat");
});
```

## 6. Add a javascript reference 

Update the file `Views\Home\Index.cshtml` with a reference to signalr javascript library:

```html
<script src="~/lib/signalr/signalr.js"></script>
```

## 7. Create a simple page

Update the file `Views\Home\Index.cshtml` with the following HTML code:

```html
<div class="signalr-demo">
    <form id="message-form">
        <input type="text" id="message-box" />
    </form>
    <hr />
    <ul id="messages"></ul>
</div>
```

and with the following javascript code:

```javascript
<script>
    // Reference to page objects
    const messageForm = document.getElementById('message-form');
    const messageBox = document.getElementById('message-box');
    const messages = document.getElementById('messages');
    
    // Create a connection
    const connection = new signalR.HubConnectionBuilder().withUrl( "/chat" )
        .configureLogging(signalR.LogLevel.Information)
        .build();

    // Listen for 'newMessage' event
    connection.on('newMessage', (sender, messageText) => {
        // Write to browser log
        console.log(`${sender} : ${ messageText }` );
        // Create a new <li> element
        const newMessage = document.createElement ( 'li' );
        // Add the new <li> element to <ul>
        newMessage.appendChild (
            document.createTextNode(`${ sender } : ${ messageText }`));
            messages.appendChild(newMessage);
    });

    // Start a connection
    connection.start()
        .then(() => console.log('connected!'))
        .catch(console.error);
    
    // Send a new message
    messageForm.addEventListener('submit', ev => {
        // Prevent the default behaviour of the form
        ev.preventDefault();
        // Get the message typed by user
        const message = messageBox.value;
        // Invoke the method SendMessage on the server
        connection.invoke('SendMessage' , message);
        // Restart the message box
        messageBox.value = '';
    });
</script>
```

## 8. Test the simple chat

Open 2 or more browsers and start typing messages. All opened browsers should now see the messages.

# Execution steps

If you clone this project you should execute the following commands in project root folder to make it work:

## 1. Update .Net packages

    dotnet restore

## 2. Update frontend libraries

    npm install

## 3. Execute the project

    dotnet run


## References

- https://www.codemag.com/Article/1807061/Build-Real-time-Applications-with-ASP.NET-Core-SignalR
- https://docs.microsoft.com/en-us/aspnet/core/signalr/javascript-client?view=aspnetcore-2.2
- https://docs.microsoft.com/en-us/aspnet/signalr/overview/getting-started/tutorial-getting-started-with-signalr-and-mvc