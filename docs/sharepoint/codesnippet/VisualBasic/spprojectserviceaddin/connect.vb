Imports System
Imports Microsoft.VisualStudio.CommandBars
Imports Extensibility
Imports EnvDTE
Imports EnvDTE80

Public Class Connect
	
    Implements IDTExtensibility2
    Implements IDTCommandTarget

    Private _applicationObject As DTE2
    Private _addInInstance As AddIn

    Public Sub New()

    End Sub

    Public Sub OnConnection(ByVal application As Object, ByVal connectMode As ext_ConnectMode, ByVal addInInst As Object, ByRef custom As Array) Implements IDTExtensibility2.OnConnection
        _applicationObject = CType(application, DTE2)
        _addInInstance = CType(addInInst, AddIn)
        If connectMode = ext_ConnectMode.ext_cm_UISetup Then

            Dim commands As Commands2 = CType(_applicationObject.Commands, Commands2)
            Dim toolsMenuName As String = "Tools"

            'Place the command on the tools menu.
            'Find the MenuBar command bar, which is the top-level command bar holding all the main menu items:
            Dim commandBars As CommandBars = CType(_applicationObject.CommandBars, CommandBars)
            Dim menuBarCommandBar As CommandBar = commandBars.Item("MenuBar")

            'Find the Tools command bar on the MenuBar command bar:
            Dim toolsControl As CommandBarControl = menuBarCommandBar.Controls.Item(toolsMenuName)
            Dim toolsPopup As CommandBarPopup = CType(toolsControl, CommandBarPopup)

            Try
                'Add a command to the Commands collection:
                Dim command As Command = commands.AddNamedCommand2(_addInInstance, "SPProjectServiceAddIn", "SPProjectServiceAddIn", "Executes the command for SPProjectServiceAddIn", True, 59, Nothing, CType(vsCommandStatus.vsCommandStatusSupported, Integer) + CType(vsCommandStatus.vsCommandStatusEnabled, Integer), vsCommandStyle.vsCommandStylePictAndText, vsCommandControlType.vsCommandControlTypeButton)

                'Find the appropriate command bar on the MenuBar command bar:
                command.AddControl(toolsPopup.CommandBar, 1)
            Catch argumentException As System.ArgumentException
                'If we are here, then the exception is probably because a command with that name
                '  already exists. If so there is no need to recreate the command and we can 
                '  safely ignore the exception.
            End Try

        End If

        TestSPService()
    End Sub

    Public Sub OnDisconnection(ByVal disconnectMode As ext_DisconnectMode, ByRef custom As Array) Implements IDTExtensibility2.OnDisconnection
    End Sub

    Public Sub OnAddInsUpdate(ByRef custom As Array) Implements IDTExtensibility2.OnAddInsUpdate
    End Sub

    Public Sub OnStartupComplete(ByRef custom As Array) Implements IDTExtensibility2.OnStartupComplete
    End Sub

    Public Sub OnBeginShutdown(ByRef custom As Array) Implements IDTExtensibility2.OnBeginShutdown
    End Sub

    Public Sub QueryStatus(ByVal commandName As String, ByVal neededText As vsCommandStatusTextWanted, ByRef status As vsCommandStatus, ByRef commandText As Object) Implements IDTCommandTarget.QueryStatus
        If neededText = vsCommandStatusTextWanted.vsCommandStatusTextWantedNone Then
            If commandName = "SPProjectServiceAddIn.Connect.SPProjectServiceAddIn" Then
                status = CType(vsCommandStatus.vsCommandStatusEnabled + vsCommandStatus.vsCommandStatusSupported, vsCommandStatus)
            Else
                status = vsCommandStatus.vsCommandStatusUnsupported
            End If
        End If
    End Sub

    Public Sub Exec(ByVal commandName As String, ByVal executeOption As vsCommandExecOption, ByRef varIn As Object, ByRef varOut As Object, ByRef handled As Boolean) Implements IDTCommandTarget.Exec
        handled = False
        If executeOption = vsCommandExecOption.vsCommandExecOptionDoDefault Then
            If commandName = "SPProjectServiceAddIn.Connect.SPProjectServiceAddIn" Then
                handled = True
                Exit Sub
            End If
        End If
    End Sub

    Private Sub TestSPService()

        '<Snippet1>
        Dim serviceProvider As Microsoft.VisualStudio.Shell.ServiceProvider = _
            New Microsoft.VisualStudio.Shell.ServiceProvider( _
                TryCast(_applicationObject, Microsoft.VisualStudio.OLE.Interop.IServiceProvider))

        Dim projectService As Microsoft.VisualStudio.SharePoint.ISharePointProjectService = _
            TryCast(serviceProvider.GetService(GetType(Microsoft.VisualStudio.SharePoint.ISharePointProjectService)), _
                Microsoft.VisualStudio.SharePoint.ISharePointProjectService)

        If projectService IsNot Nothing Then
            projectService.Logger.WriteLine("This message was written by using the SharePoint project service.", _
                Microsoft.VisualStudio.SharePoint.LogCategory.Message)
        End If
        '</Snippet1>

        AddHandler projectService.ProjectAdded, AddressOf projectService_ProjectAdded
    End Sub

    '<Snippet2>
    Private Sub projectService_ProjectAdded(ByVal sender As Object, _
        ByVal e As Microsoft.VisualStudio.SharePoint.SharePointProjectEventArgs)

        Dim dteProject As EnvDTE.Project = e.Project.ProjectService.Convert( _
            Of Microsoft.VisualStudio.SharePoint.ISharePointProject, EnvDTE.Project)(e.Project)
        If dteProject IsNot Nothing Then
            ' Use the Visual Studio automation object model to add a folder to the project.
            dteProject.ProjectItems.AddFolder("Data")
        End If
    End Sub
    '</Snippet2>
End Class
