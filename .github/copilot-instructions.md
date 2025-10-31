# EmailSenderDLL - Instructions Copilot

## Exigences du projet

**Type:** DLL VB.NET (Class Library)  
**Framework:** .NET Framework 4.8  
**Langage:** Visual Basic .NET

### Fonctionnalités principales
- Envoi d'emails via SendGrid REST API
- 3 types d'emails avec templates HTML distincts:
  - **Info** (Bleu #2196F3)
  - **Erreur** (Rouge #f44336)
  - **Urgence** (Orange #ff9800)
- Support CC (copie carbone) et BCC (copie carbone invisible)
- Signatures personnalisables
- Affichage de pièces jointes (liste informative)
- Templates HTML responsive

### Dépendances
- **Newtonsoft.Json** (pour sérialisation JSON)

### Configuration SendGrid
- API Key: `***REMOVED***`
- Sender Email: `***REMOVED***`
- Sender Name: `Tech Dev DAAM`
- Endpoint: `https://api.sendgrid.com/v3/mail/send`

## Structure du projet

```
SenderEmail/
├── .github/
│   └── copilot-instructions.md
├── EmailSenderDLL/
│   ├── EmailSender.vb
│   ├── EmailSenderDLL.vbproj
│   └── bin/Release/net48/
│       ├── EmailSenderDLL.dll
│       ├── Newtonsoft.Json.dll
│       └── EmailSenderDLL.xml
├── Documentation/
│   ├── README.md
│   ├── GUIDE_INTEGRATION_PAS_A_PAS.md
│   ├── DEMARRAGE_RAPIDE.md
│   ├── GUIDE_UTILISATION.md
│   └── ExempleUtilisation.vb
└── Tests/
    └── TestEmail.vb
```

## API EmailSender

### Classe principale
```vb
Public Class EmailSender
    Public Sub New(apiKey As String, fromEmail As String, fromName As String)
    
    Public Async Function EnvoyerEmailAsync(
        destinataire As String,
        sujet As String,
        message As String,
        Optional typeEmail As TypeEmail = TypeEmail.Info,
        Optional signature As String = Nothing,
        Optional pieceJointes As List(Of String) = Nothing,
        Optional cc As List(Of String) = Nothing,
        Optional cci As List(Of String) = Nothing
    ) As Task(Of Boolean)
End Class

Public Enum TypeEmail
    Info = 0
    Erreur = 1
    Urgence = 2
End Enum
```

## Règles de développement

1. **Utiliser System.Net.Http** pour les appels REST API
2. **Générer des templates HTML inline** (pas de fichiers externes)
3. **Gestion d'erreurs complète** avec messages explicites
4. **Asynchrone** pour toutes les opérations réseau
5. **Documentation XML** pour IntelliSense
6. **Exemples concrets** dans la documentation
