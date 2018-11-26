# Securotech-ACW
Distributed Systems Coursework

Project contains 2 Visual studio solutions.  

1 - Client  

Client Commands: 

Talkback Sort <ints> - Returns a sorted array
Talkback Hello - Returns "Hello World"

User Get <Name> - Checks if the user exists and returns True/False
User Post <Name> - Returns Created users API key and saves it for later user
	Only one API key is stored at a time
User Delete <Name> - Deletes the user if the API key is saved

Protected Hello - Returns Hello <Name>
Protected Sha1 <Message> - Returns the message encrypted by Sha1
Protected Sha256 <Message> - Returns the message encrypted by Sha256
Protected Get - Returns the public key for the server and saves it
Protected Sign Message - Sends a message to the server for signing and verifies it with the servers public key

Client Features: Talkback, User, Protected

2 - Server  
Server Features:
