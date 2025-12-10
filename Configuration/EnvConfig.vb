Imports System
Imports System.IO

''' <summary>
''' Gestionnaire de configuration sécurisée pour EmailSenderDLL
''' Charge les variables d'environnement depuis le fichier .env
''' </summary>
Public Class EnvConfig
    Private Shared _isLoaded As Boolean = False
    Private Shared ReadOnly _lockObject As New Object()

    ''' <summary>
    ''' Charge les variables d'environnement depuis le fichier .env
    ''' </summary>
    ''' <param name="envFilePath">Chemin optionnel vers le fichier .env (par défaut: racine du projet)</param>
    Public Shared Sub LoadEnvFile(Optional envFilePath As String = Nothing)
        SyncLock _lockObject
            If _isLoaded Then Return

            If String.IsNullOrEmpty(envFilePath) Then
                ' Recherche du fichier .env dans les répertoires parents
                Dim currentDir As String = Directory.GetCurrentDirectory()
                envFilePath = Path.Combine(currentDir, ".env")

                Dim maxLevels As Integer = 5
                Dim level As Integer = 0

                While Not File.Exists(envFilePath) AndAlso level < maxLevels
                    currentDir = Directory.GetParent(currentDir)?.FullName
                    If currentDir Is Nothing Then Exit While
                    envFilePath = Path.Combine(currentDir, ".env")
                    level += 1
                End While
            End If

            If Not File.Exists(envFilePath) Then
                Throw New FileNotFoundException(
                    $"Fichier .env introuvable. " &
                    $"Créez un fichier .env à partir de .env.example avec vos credentials SendGrid.", 
                    ".env")
            End If

            ' Lecture et parsing du fichier .env
            For Each line In File.ReadAllLines(envFilePath)
                Dim trimmedLine = line.Trim()
                
                ' Ignorer les commentaires et lignes vides
                If String.IsNullOrWhiteSpace(trimmedLine) OrElse trimmedLine.StartsWith("#") Then
                    Continue For
                End If

                ' Parser KEY=VALUE
                Dim separatorIndex = trimmedLine.IndexOf("="c)
                If separatorIndex > 0 Then
                    Dim key = trimmedLine.Substring(0, separatorIndex).Trim()
                    Dim value = trimmedLine.Substring(separatorIndex + 1).Trim()
                    
                    ' Retirer les guillemets optionnels
                    If value.StartsWith("""") AndAlso value.EndsWith("""") Then
                        value = value.Substring(1, value.Length - 2)
                    End If

                    Environment.SetEnvironmentVariable(key, value)
                End If
            Next

            _isLoaded = True
        End SyncLock
    End Sub

    ''' <summary>
    ''' Récupère une variable d'environnement avec validation
    ''' </summary>
    Public Shared Function GetRequired(key As String) As String
        Dim value = Environment.GetEnvironmentVariable(key)
        
        If String.IsNullOrWhiteSpace(value) Then
            Throw New InvalidOperationException(
                $"Variable d'environnement '{key}' manquante. " &
                $"Vérifiez votre fichier .env ou vos variables d'environnement système.")
        End If

        Return value
    End Function

    ''' <summary>
    ''' Récupère une variable d'environnement optionnelle
    ''' </summary>
    Public Shared Function GetOptional(key As String, Optional defaultValue As String = Nothing) As String
        Dim value = Environment.GetEnvironmentVariable(key)
        Return If(String.IsNullOrWhiteSpace(value), defaultValue, value)
    End Function
End Class
