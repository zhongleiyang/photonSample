1 copy ChatServer.dll ExitGamesLibs.dll Photon.SocketServer.dll
PhotonHostRuntimeInterfaces.dll to deploy/ChatServer/bin

2 add these lines
<Application
        Name="ChatServer"
        BaseDirectory="ChatServer"
        Assembly="ChatServer"
        Type="ChatServer" />
to PhotonServer.config file