VERSION 5.00
Begin VB.Form frmRelFollo 
   ClientHeight    =   2760
   ClientLeft      =   2460
   ClientTop       =   1800
   ClientWidth     =   11985
   ForeColor       =   &H80000008&
   KeyPreview      =   -1  'True
   LinkTopic       =   "RelFollo"
   LockControls    =   -1  'True
   MDIChild        =   -1  'True
   ScaleHeight     =   2760
   ScaleWidth      =   11985
   Begin IRRIG.GPainel Painel 
      Height          =   2730
      Index           =   0
      Left            =   15
      TabIndex        =   11
      TabStop         =   0   'False
      Top             =   15
      Width           =   11955
      _ExtentX        =   21087
      _ExtentY        =   4815
      BevelOuter      =   0
      Stretch         =   -1  'True
      Begin IRRIG.GBotao botConfImp 
         Height          =   360
         Left            =   11205
         TabIndex        =   7
         Top             =   2145
         Width           =   360
         _ExtentX        =   0
         _ExtentY        =   0
         ForeColor       =   0
         BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         CaptionOffset   =   50
      End
      Begin IRRIG.GBotao botOk 
         Height          =   360
         Left            =   1620
         TabIndex        =   0
         Top             =   1005
         Width           =   1485
         _ExtentX        =   0
         _ExtentY        =   0
         ForeColor       =   0
         BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
            Name            =   "Microsoft Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Caption         =   "Prossegue"
         CaptionAlign    =   4
         CaptionOffset   =   50
      End
      Begin IRRIG.GBotao botCancela 
         Height          =   360
         Left            =   3120
         TabIndex        =   8
         Top             =   1005
         Width           =   1485
         _ExtentX        =   0
         _ExtentY        =   0
         ForeColor       =   0
         BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
            Name            =   "Microsoft Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Caption         =   "Cancela"
         CaptionAlign    =   4
         CaptionOffset   =   50
      End
      Begin VB.PictureBox picBox 
         BorderStyle     =   0  'None
         ForeColor       =   &H80000008&
         Height          =   1425
         Index           =   0
         Left            =   0
         ScaleHeight     =   1425
         ScaleWidth      =   1455
         TabIndex        =   12
         Top             =   75
         Width           =   1455
      End
      Begin IRRIG.GPainel Painel 
         Height          =   240
         Index           =   1
         Left            =   4440
         TabIndex        =   14
         TabStop         =   0   'False
         Top             =   2220
         Width           =   2280
         _ExtentX        =   4022
         _ExtentY        =   423
         BevelOuter      =   0
         Stretch         =   -1  'True
         Begin VB.OptionButton opcPainel1Cp 
            Caption         =   "Janela"
            BeginProperty Font 
               Name            =   "Microsoft Sans Serif"
               Size            =   9.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            ForeColor       =   &H00800000&
            Height          =   240
            Index           =   0
            Left            =   0
            TabIndex        =   1
            TabStop         =   0   'False
            Top             =   0
            Width           =   915
         End
         Begin VB.OptionButton opcPainel1Cp 
            Caption         =   "Impressora"
            BeginProperty Font 
               Name            =   "Microsoft Sans Serif"
               Size            =   9.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            ForeColor       =   &H00800000&
            Height          =   240
            Index           =   1
            Left            =   990
            TabIndex        =   2
            TabStop         =   0   'False
            Top             =   0
            Width           =   1320
         End
         Begin VB.Label labopcPainel1 
            BorderStyle     =   1  'Fixed Single
            Height          =   120
            Left            =   675
            TabIndex        =   15
            Top             =   180
            Visible         =   0   'False
            Width           =   240
         End
      End
      Begin IRRIG.GPainel Painel 
         Height          =   240
         Index           =   2
         Left            =   1005
         TabIndex        =   17
         TabStop         =   0   'False
         Top             =   2190
         Width           =   2010
         _ExtentX        =   3545
         _ExtentY        =   423
         BevelOuter      =   0
         Stretch         =   -1  'True
         Begin VB.OptionButton opcPainel2Cp 
            Caption         =   "Todas"
            BeginProperty Font 
               Name            =   "Microsoft Sans Serif"
               Size            =   9.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            ForeColor       =   &H00800000&
            Height          =   240
            Index           =   0
            Left            =   0
            TabIndex        =   3
            TabStop         =   0   'False
            Top             =   0
            Width           =   905
         End
         Begin VB.OptionButton opcPainel2Cp 
            Caption         =   "Páginas"
            BeginProperty Font 
               Name            =   "Microsoft Sans Serif"
               Size            =   9.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            ForeColor       =   &H00800000&
            Height          =   240
            Index           =   1
            Left            =   975
            TabIndex        =   4
            TabStop         =   0   'False
            Top             =   0
            Width           =   1050
         End
         Begin VB.Label labopcPainel2 
            BorderStyle     =   1  'Fixed Single
            Height          =   120
            Left            =   665
            TabIndex        =   18
            Top             =   180
            Visible         =   0   'False
            Width           =   240
         End
      End
      Begin VB.TextBox txtCp 
         Appearance      =   0  'Flat
         BeginProperty Font 
            Name            =   "Microsoft Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   315
         Index           =   0
         Left            =   3090
         MaxLength       =   254
         TabIndex        =   5
         TabStop         =   0   'False
         Top             =   2190
         Width           =   465
      End
      Begin VB.VScrollBar scrVtxtCampo1 
         Height          =   285
         LargeChange     =   10
         Left            =   8190
         Max             =   0
         Min             =   32767
         TabIndex        =   20
         TabStop         =   0   'False
         Top             =   2205
         Width           =   195
      End
      Begin VB.TextBox txtCp 
         Appearance      =   0  'Flat
         BorderStyle     =   0  'None
         BeginProperty Font 
            Name            =   "Microsoft Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   285
         Index           =   1
         Left            =   7845
         MaxLength       =   254
         MultiLine       =   -1  'True
         TabIndex        =   6
         TabStop         =   0   'False
         Top             =   2205
         Width           =   330
      End
      Begin VB.TextBox txtCp 
         Appearance      =   0  'Flat
         BeginProperty Font 
            Name            =   "Microsoft Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   360
         Index           =   2
         Left            =   6200
         MaxLength       =   254
         TabIndex        =   9
         TabStop         =   0   'False
         Top             =   1005
         Width           =   2100
      End
      Begin VB.TextBox txtCp 
         Appearance      =   0  'Flat
         BeginProperty Font 
            Name            =   "Microsoft Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   360
         Index           =   3
         Left            =   9500
         MaxLength       =   254
         TabIndex        =   10
         TabStop         =   0   'False
         Top             =   1005
         Width           =   2100
      End
      Begin VB.Label Label 
         AutoSize        =   -1  'True
         Caption         =   "Situação:"
         BeginProperty Font 
            Name            =   "Microsoft Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00800000&
         Height          =   240
         Index           =   7
         Left            =   9500
         TabIndex        =   28
         Top             =   1470
         Width           =   900
      End
      Begin VB.ComboBox cboSituacao 
         BackColor       =   &H00FFFFFF&
         BeginProperty Font 
            Name            =   "Microsoft Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   315
         Left            =   9500
         Style           =   2  'Dropdown List
         TabIndex        =   11
         Top             =   1695
         Width           =   2100
      End
      Begin VB.Label Label 
         AutoSize        =   -1  'True
         Caption         =   "Destino:"
         BeginProperty Font 
            Name            =   "Microsoft Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00800000&
         Height          =   240
         Index           =   0
         Left            =   3630
         TabIndex        =   13
         Top             =   2220
         Width           =   735
      End
      Begin VB.Label Label 
         AutoSize        =   -1  'True
         Caption         =   "Intervalo:"
         BeginProperty Font 
            Name            =   "Microsoft Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00800000&
         Height          =   240
         Index           =   1
         Left            =   120
         TabIndex        =   16
         Top             =   2190
         Width           =   810
      End
      Begin VB.Label labFdo1 
         Appearance      =   0  'Flat
         BackColor       =   &H80000005&
         BorderStyle     =   1  'Fixed Single
         Height          =   315
         Left            =   7830
         TabIndex        =   19
         Top             =   2190
         Width           =   570
      End
      Begin VB.Label Label 
         AutoSize        =   -1  'True
         Caption         =   "Nº Copias:"
         BeginProperty Font 
            Name            =   "Microsoft Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H80000008&
         Height          =   240
         Index           =   2
         Left            =   6795
         TabIndex        =   21
         Top             =   2220
         Width           =   960
      End
      Begin VB.Label Label 
         AutoSize        =   -1  'True
         Caption         =   "Impres.:"
         BeginProperty Font 
            Name            =   "Microsoft Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H80000008&
         Height          =   240
         Index           =   3
         Left            =   8475
         TabIndex        =   22
         Top             =   2220
         Width           =   705
      End
      Begin VB.Label Label 
         Appearance      =   0  'Flat
         BorderStyle     =   1  'Fixed Single
         BeginProperty Font 
            Name            =   "Microsoft Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FF0000&
         Height          =   300
         Index           =   4
         Left            =   9255
         TabIndex        =   23
         Top             =   2190
         Width           =   1920
      End
      Begin VB.Label labFdo2 
         BackStyle       =   0  'Transparent
         Caption         =   "D.Inicial"
         BeginProperty Font 
            Name            =   "Microsoft Sans Serif"
            Size            =   12
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   400
         Left            =   5100
         TabIndex        =   24
         Top             =   1005
         Width           =   1000
      End
      Begin VB.Label labFdo3 
         BackStyle       =   0  'Transparent
         Caption         =   "D.Final"
         BeginProperty Font 
            Name            =   "Microsoft Sans Serif"
            Size            =   12
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   315
         Left            =   8500
         TabIndex        =   25
         Top             =   1005
         Width           =   900
      End
      Begin VB.Label Label 
         Alignment       =   2  'Center
         AutoSize        =   -1  'True
         Caption         =   "Controle de Pedidos - Vendidos"
         BeginProperty Font 
            Name            =   "Microsoft Sans Serif"
            Size            =   20.25
            Charset         =   0
            Weight          =   700
            Underline       =   -1  'True
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00000000&
         Height          =   465
         Index           =   5
         Left            =   1575
         TabIndex        =   26
         Top             =   420
         Width           =   6195
      End
      Begin VB.Line Linha 
         Index           =   0
         X1              =   75
         X2              =   11550
         Y1              =   2130
         Y2              =   2130
      End
      Begin VB.Label Label 
         AutoSize        =   -1  'True
         Caption         =   "-"
         ForeColor       =   &H80000008&
         Height          =   195
         Index           =   6
         Left            =   11640
         TabIndex        =   27
         Top             =   2505
         Width           =   45
      End
   End
End
Attribute VB_Name = "frmRelFollo"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'* Sistema...: SISTEMA IRRIGAÇÃO PENÁPOLIS
'* Empresa...: IRRIGAÇÃO PENÁPOLIS
'* Módulo....: RelFollo
'* Função....: Controle - Pedidos Faturados
'* CopyRight.: (C)2025 IRRIGAÇÃO PENÁPOLIS
'* Criação...: GAS-2007 - Gerador Automático de Sistemas
'* Data......: 24/06/2025 17:02:07
'* * * * * * *

Option Explicit
DefInt A-Z

Public vgSituacao As Integer                      'situação de edição que do módulo
Public vgCaracteristica As Integer                'caracteristica do módulo
Public vgTipo As Integer                          'tipo do módulo
Public vgPriVez As Integer                        'flag de carregamento do módulo
Public vgFormID As Long                           'identificador único para o módulo
Public vgIdentTab As String                       'nome da tabela principal do módulo
Public vgEmBrowse As Integer                      'flag se o módulo esta em grade
Public vgTemFiltro As Integer                     'flag se tem ou não filtro no módulo
Public vgFiltraSoUm As String                     'flag para filtrar só um registro
Public vgCancelou As Integer                      'flag cancela ou nao o processo do módulo
Public vgQueryAtual As String                     'expressão SQL definada para o módulo
Public vgUltimoTabIndex As Integer                'último campo com foco do módulo

Public vgUltimoFiltro As String                   'último filtro definido no módulo
Public vgUltimaOrdem As String                    'última ordenação feita no módulo
Public vgUltimoFiltroComTit As String             'titulo do último filtro definido no módulo
Public vgUltimaOrdemComTit As String              'titulo da última ordenação feita no módulo
Public vgSQL As String                            'expressão SQL que define o módulo
Public vgFP As New frmPreview                     'formuário de preview do relatório
Public vgVisivel As Boolean                       'se a tela de parâmetros do relatório esta visível
Public vgPosLin As Single                         'posição atual de impressão
Public vgTooltips As New cTooltips                'classe de ajuda para os controes do módulo
Public vgFG As frmGauge                           'formulário do gauge
Public vgQReg As Long                             'quantidade de já feira para o gauge

Dim vgBookPg() As Variant                         'marcador do registro do inicio da página
Dim vgPodeVoltar() As Boolean                     'se pode ou não retroceder para esta página
Dim vgDataInicial As Variant                      'data que iniciou o relatório
Dim Dt_Inicial As Variant, Dt_Final As Variant    'filtros de data (Data Inicial / Data Final) para o relatório
Dim vgPrimeiroCab As Integer                      'flag se ja foi impresso o primeiro cabeçlho do relatório
Dim vgFiltroInicial As String                     'filtro inicial do relatório
Dim vgOrdemInicial As String                      'ordem incial do relatório
Dim txtCampo(3) As New FormataCampos              'classe dos compos tipo texto do módulo
Dim vgWidthRel As Single                          'lagura do relatório
Dim vgHeightRel As Single                         'altura do relatório
Dim vgPagRef() As Integer                         'Ordem da página dentro do intervalo
Dim opcPainel1(1) As New FormataCampos
Dim opcPainel2(1) As New FormataCampos
Dim vgDestRel As Integer, vgModoInt As Integer, vgIntPag As String
Dim vgCopias As Integer, Ajuste As String
Dim vgMemo0() As Long, vgMemo1() As Long
Public lblAjuste As Object
Dim Follow_Up_Vendas As New GRecordSet
Dim vgFormatandoData As Boolean
Dim vgFiltroSituacaoOpcao As Integer          '0-Todos, 1-Apenas entregues, 2-Em fábrica

Private Function Filtro() As String
Filtro = " [Venda Fechada] = 1 And Stat <> 'Vinculado' AND [Seqüência Do Orçamento] <> 109278 AND [Seqüência Do Orçamento] <> 124421 AND [Seqüência Do Orçamento] <> 124270 " 'Removendo o orçamento markamix do relatório sem desmarcar o campo venda fechada
 'If Estoque Then
    Filtro = Filtro & " And [Follow Up Vendas].[Razão Social] <> 'Irrigação Penápolis Ind. e Com. Ltda'"
 'End If
 If Not Vazio(Dt_Inicial) And Not Vazio(Dt_Final) Then
    ' Use upper-bound exclusive (next day) so the entire Dt_Final day is included
    Filtro = Filtro & " And [Data de Emissão] >= " & D(Dt_Inicial) & " And [Data de Emissão] < " & D(DateAdd("d", 1, Dt_Final))
 End If
 Select Case vgFiltroSituacaoOpcao
    Case 1
       Filtro = Filtro & " And Stat In ('Entregue','Entregues')"
    Case 2
       Filtro = Filtro & " And Stat In ('Vencidos','A Vencer','Aguardando Retirada')"
 End Select
End Function

Private Function TomadorFrete() As String
 Dim Tb As New GRecordSet
 
  Set Tb = vgDb.OpenRecordSet("SELECT * From Orçamento WHERE [Seqüência Do Orçamento] = " & Follow_Up_Vendas![Seqüência do Orçamento] & " And Observação LIKE '%cif%' ")
      
      If Follow_Up_Vendas!Stat = "Estoque" Then
         TomadorFrete = ""
         Exit Function
      End If
      
      If Tb.RecordCount > 0 Then
         TomadorFrete = "CIF"
      Else
         TomadorFrete = "FOB"
      End If
  
End Function

Private Function MostraTotal() As Currency
   Dim tb As GRecordSet
   Dim total As Currency

   On Error GoTo TrataErro_MostraTotal

   Set tb = vgDb.OpenRecordSet("SELECT [Valor Total do Orçamento] AS Total FROM Orçamento WHERE [Seqüência Do Orçamento] = " & Follow_Up_Vendas![Seqüência do Orçamento])
   If Not tb Is Nothing Then
      If tb.RecordCount > 0 Then
         If Not IsNull(tb!Total) Then total = tb!Total
      End If
   End If

Saida_MostraTotal:
   Set tb = Nothing
   MostraTotal = total
   Exit Function

TrataErro_MostraTotal:
   total = 0
   Resume Saida_MostraTotal
End Function


Private Function MostraPago() As Currency
   Dim tb As GRecordSet
   Dim valorBase As Currency
   Dim seqOrcamento As Long

   On Error GoTo TrataErro_MostraPago

   If IsNull(Follow_Up_Vendas![Seqüência do Orçamento]) Then
      seqOrcamento = 0
   Else
      seqOrcamento = CLng(Follow_Up_Vendas![Seqüência do Orçamento])
   End If

   Set tb = vgDb.OpenRecordSet("SELECT SUM([Valor Total]) AS Total FROM [Movimentação da Conta Corrente] WHERE [Seqüência Do Orçamento] = " & seqOrcamento)
   If Not tb Is Nothing Then
      If tb.RecordCount > 0 Then
         If Not IsNull(tb!Total) Then valorBase = tb!Total
      End If
   End If

   MostraPago = valorBase

   If seqOrcamento = 113114 Or seqOrcamento = 113364 Then
      MostraPago = MostraTotal() / 2
   ElseIf seqOrcamento = 123902 Then
      MostraPago = valorBase - 68579
   ElseIf seqOrcamento = 124773 Then
      MostraPago = valorBase + 535000
   ElseIf seqOrcamento = 124775 Then
      MostraPago = valorBase + 8400
   ElseIf seqOrcamento = 125031 Then
      MostraPago = valorBase + 211500
   ElseIf seqOrcamento = 125194 Then
      MostraPago = valorBase + 3711.36
   ElseIf seqOrcamento = 125227 Then
      MostraPago = valorBase + 76300
   ElseIf seqOrcamento = 125229 Then
      MostraPago = valorBase + 15250
   ElseIf seqOrcamento = 123428 Then
      MostraPago = valorBase + 13500
   ElseIf seqOrcamento = 119853 Then
      MostraPago = valorBase + 117000
   ElseIf seqOrcamento = 119854 Then
      MostraPago = valorBase + 126000
   ElseIf seqOrcamento = 125406 Then
      MostraPago = valorBase + 230.77
   ElseIf seqOrcamento = 123988 Then
      MostraPago = valorBase + 16200
   ElseIf seqOrcamento = 124497 Then
      MostraPago = valorBase + 100000
   ElseIf seqOrcamento = 124499 Then
      MostraPago = valorBase + 179500
   ElseIf seqOrcamento = 124722 Then
      MostraPago = valorBase + 80400
   ElseIf seqOrcamento = 124699 Then
      MostraPago = valorBase + 70000
   ElseIf seqOrcamento = 124557 Then
      MostraPago = valorBase + 120000
   ElseIf seqOrcamento = 125027 Then
      MostraPago = valorBase + 3178.9
   End If

Saida_MostraPago:
   Set tb = Nothing
   Exit Function

TrataErro_MostraPago:
   MostraPago = valorBase
   Resume Saida_MostraPago
End Function

Private Function NzTexto(ByVal valor As Variant) As String
   If IsNull(valor) Then
      NzTexto = ""
   Else
      NzTexto = Trim$(CStr(valor))
   End If
End Function

Private Function LimitaTexto(ByVal valor As Variant, ByVal maxChars As Integer) As String
   Dim texto As String
   texto = NzTexto(valor)
   If maxChars > 3 And Len(texto) > maxChars Then
      LimitaTexto = Left$(texto, maxChars - 3) & "..."
   Else
      LimitaTexto = texto
   End If
End Function

Private Function CorDeStatus(ByVal descricao As String) As String
   Select Case UCase$(Trim$(descricao))
      Case "VENCIDOS"
         CorDeStatus = "F65535"
      Case "A VENCER"
         CorDeStatus = "F65280"
      Case "AGUARDANDO RETIRADA"
         CorDeStatus = "F-2147483646"
      Case "ENTREGUE", "ENTREGUES"
         CorDeStatus = "F16777164"
      Case Else
         CorDeStatus = ""
   End Select
End Function
'prepara o intervalo de páginas a ser impresso
Public Property Let vgIntervalPag(ByVal NovoValor As String)
   Dim x As String, z As String, i As Integer, j As Integer, k As Integer
   ReDim vgPagRef(0) As Integer
   vgIntPag$ = NovoValor$
   x$ = Substitui(vgIntPag$, ";", ",", UM_A_UM)
   j = 0
   Do While Len(x$) > 0
      z$ = Parse(x$, ",")
      i = InStr(z$, "-")
      If i > 0 Then
         k = val(Mid(z$, i + 1))
         i = val(Left(z$, i - 1))
      Else
         i = val(z$)
         k = val(z$)
      End If
      For i = i To k Step ((i > k) ^ (i > k))
         j = j + 1
         ReDim Preserve vgPagRef(j) As Integer
         vgPagRef(j) = i
      Next
   Loop
End Property

'inicializa variaveis (apelidos) coms os campos correspondentes
Private Sub InicializaApelidos(vgComOQue As Integer)
   On Error Resume Next                           'prepara para possíveis erros
   vgDestRel = val(labopcPainel1.Caption)
   vgModoInt = val(labopcPainel2.Caption)
   vgIntPag = txtCampo(0).Value
   vgCopias = txtCampo(1).Value
   If cboSituacao.ListIndex < 0 Then cboSituacao.ListIndex = 0
   vgFiltroSituacaoOpcao = cboSituacao.ListIndex
   ' Garantir que os campos de data estejam formatados (caso o usuário não tenha perdido o foco)
   On Error Resume Next
   Call FormataDataCampo(2)
   Call FormataDataCampo(3)
   On Error GoTo 0
      ' Tentar ler diretamente do texto do controle e montar uma Data com DateSerial
      Dt_Inicial = Null
      Dt_Final = Null
      On Error Resume Next
      If Len(Trim$(txtCp(2).Text)) = 10 Then
         Dim di As Integer, mi As Integer, yi As Integer
         di = val(Left$(txtCp(2).Text, 2))
         mi = val(Mid$(txtCp(2).Text, 4, 2))
         yi = val(Right$(txtCp(2).Text, 4))
         Dt_Inicial = DateSerial(yi, mi, di)
      End If
      If Len(Trim$(txtCp(3).Text)) = 10 Then
         Dim df As Integer, mf As Integer, yf As Integer
         df = val(Left$(txtCp(3).Text, 2))
         mf = val(Mid$(txtCp(3).Text, 4, 2))
         yf = val(Right$(txtCp(3).Text, 4))
         Dt_Final = DateSerial(yf, mf, df)
      End If
      ' DEBUG output removed
      On Error GoTo 0
   vgIntervalPag = vgIntPag
   If Err Then Err.Clear                          'se deu algum erro, vamos resetá-lo
End Sub

' Pré-preenche campos de data (se desejado)
Private Sub PreencheDatasPadrao()
   On Error Resume Next
   ' Preenche Data Inicial e Final com UltD(Dt_Inicial) como exemplo
   txtCampo(2).Value = UltD(Dt_Inicial)
   txtCampo(3).Value = UltD(Dt_Inicial)
   On Error GoTo 0
End Sub

'executa processos/validacoes nos campos do formuário
Public Function Executar(vgOq As String) As String
   Dim vgMsg As String, vgOk As Integer, vgNVez As Integer
   On Error GoTo DeuErro                          'fica na espera de um erro...
   vgMsg$ = ""                                    'retorna uma msg dizendo o motivo
   vgOk = True                                    'se a validação esta OK
   vgNVez = 0                                     'porque não fez o processo/validacoes
   If vgOq = VALIDACOES Then
      InicializaApelidos COM_TEXTBOX
      If vgModoInt = 1 Then
         vgOk = (Len(vgIntPag) > 0)
         vgMsg$ = "Esta informação é necessária"
         If Not vgOk Then txtCampo(0).SetFocus
      End If
      If vgOk Then
         If vgDestRel = 1 Then
            vgOk = (vgCopias > 0)
            vgMsg$ = "Número de cópias tem que ser maior que ZERO!"
            If Not vgOk Then txtCampo(1).SetFocus
         End If
      End If
      If vgOk Then vgMsg$ = ""
      If vgOk Then
         'Validação simples de intervalo de datas, se preenchidas
         If Not Vazio(Dt_Inicial) Or Not Vazio(Dt_Final) Then
            If Vazio(Dt_Inicial) Or Vazio(Dt_Final) Then
               vgOk = False
               vgMsg$ = "Preencha Data Inicial e Data Final"
               If Vazio(Dt_Inicial) Then txtCampo(2).SetFocus Else txtCampo(3).SetFocus
            Else
               vgOk = (CDate(Dt_Final) >= CDate(Dt_Inicial))
               If Not vgOk Then
                  vgMsg$ = "Data Final deve ser maior ou igual à Data Inicial"
                  txtCampo(3).SetFocus
               End If
            End If
         End If
      End If
      DoEvents
   ElseIf vgOq = INICIALIZACOES Then
      InicializaApelidos COM_TEXTBOX
      On Error Resume Next
      opcPainel1(0).Value = True
      opcPainel1(0).CtPri.TabStop = False
      opcPainel2(0).Value = True
      opcPainel2(0).CtPri.TabStop = False
   ' (sem pré-preenchimento) campos de data ficam vazios para o usuário escolher
   ElseIf vgOq = INI_APELIDOS Then
      InicializaApelidos COM_REGISTRO
      ExecutaVisivel
      ExecutaPreValidacao
   End If
   Executar = vgMsg$                              'prepara saida da função
   Exit Function                                  'e cai fora...

DeuErro:
   Select Case Err                                'vamos verificar se deu algum erro
      Case 3197                                   'registro acabou de ser alterado por outro usuário
         Resume
      Case 3046, 3158, 3186, 3187, 3188, 3189, 3218, 3260
         vgNVez = vgNVez + 1                      'outro usuário acessando
         If vgNVez <= 10 Then                     'vamos tentar 10 vezes
            Delay 0.5                             'mais antes espera 1/2 segundo
            Resume                                'antes de tentar
         End If
   End Select
   Executar = Err.Source + "|" + Trim$(Str$(Err)) + "|" + Error$ 'não teve jeito o erro não pode ser evitado...
End Function

'executa a pré-validação dos campos
Private Sub ExecutaPreValidacao()
   Dim Ok As Boolean
   On Error Resume Next                           'prepara para possiveis erros
   Ok = (vgModoInt = 1)
   txtCampo(0).Enabled = (Ok And txtCampo(0).Editable)
   Ok = (vgDestRel = 1)
   txtCampo(1).Enabled = (Ok And txtCampo(1).Editable)
   If Err Then Err.Clear                          'se houve erro, limpa...
End Sub

'coloca os campos visíveis segundo a condição
Private Sub ExecutaVisivel()
   On Error Resume Next                           'prepara para possiveis erros
   Label(6).Visible = (False)
   If Err Then Err.Clear                          'se houve erro, limpa...
End Sub

Private Sub cboSituacao_Click()
   On Error Resume Next
   vgFiltroSituacaoOpcao = cboSituacao.ListIndex
   If Err Then Err.Clear
End Sub

'evento - quando o conteúdo do campo for alterado
Private Sub txtCp_Change(Index As Integer)
   If vgPriVez Or txtCampo(Index).PriVez Then Exit Sub
   InicializaApelidos COM_TEXTBOX                         'inicializa apelidos com o que esta sendo digitado
   txtCampo(Index).Change
End Sub

'evento - quando o campo receber o foco
Private Sub txtCp_GotFocus(Index As Integer)
         On Error Resume Next
         Select Case Index
         Case 1
            If ValBrasil(txtCp(1).Text) = 0 Then
               txtCampo(1).Value = 1
               txtCp_Change Index
               InicializaApelidos COM_TEXTBOX
               ExecutaVisivel
               ExecutaPreValidacao
            End If
      End Select
   txtCampo(Index).GotFocus
End Sub


'evento - quando qq tecla for digitada no campo
Private Sub txtCp_KeyDown(Index As Integer, KeyCode As Integer, Shift As Integer)
   txtCampo(Index).KeyDown KeyCode, Shift
End Sub

'evento - quando qq tecla for digitada no campo
Private Sub txtCp_KeyPress(Index As Integer, KeyAscii As Integer)
   txtCampo(Index).KeyPress KeyAscii
End Sub

'evento - quando o campo perder o foco
Private Sub txtCp_LostFocus(Index As Integer)
   txtCampo(Index).LostFocus                      'executa LostFocus na classe
   ' Ao perder foco, formata o campo de data (índices 2 e 3)
   If Index = 2 Or Index = 3 Then
      On Error Resume Next
      Call FormataDataCampo(Index)
      On Error GoTo 0
   End If
   ExecutaVisivel                                 'torna camos visiveis
   ExecutaPreValidacao                            'habilita/desabilita campos
End Sub


'evento - quando uma opção for selecionada
Private Sub opcPainel1Cp_Click(Index As Integer)
   If vgPriVez Then Exit Sub
   If opcPainel1(Index).Locked Then
      opcPainel1(val(labopcPainel1.Caption)).Value = True
   Else
      If val(labopcPainel1.Caption) <> opcPainel1(Index).BookMark Then
         labopcPainel1.Caption = Str$(opcPainel1(Index).BookMark)
         InicializaApelidos COM_TEXTBOX
         ExecutaVisivel
         ExecutaPreValidacao
         opcPainel1(Index).Change
      End If
   End If
   opcPainel1(0).CtPri.TabStop = False
   opcPainel1(1).CtPri.TabStop = False
   opcPainel2(0).CtPri.TabStop = False
   opcPainel2(1).CtPri.TabStop = False
End Sub

' Formata o texto do campo de data no formato dd/mm/yyyy quando perder o foco
Private Sub FormataDataCampo(Index As Integer)
   Dim s As String, i As Integer, ApenasNums As String, d As String, m As String, a As String, dt As Date
   s = txtCp(Index).Text
   ApenasNums = ""
   For i = 1 To Len(s)
      If Mid$(s, i, 1) >= "0" And Mid$(s, i, 1) <= "9" Then
         ApenasNums = ApenasNums & Mid$(s, i, 1)
      End If
   Next
   If Len(ApenasNums) = 8 Then
      d = Left$(ApenasNums, 2)
      m = Mid$(ApenasNums, 3, 2)
      a = Mid$(ApenasNums, 5, 4)
      ' Monta diretamente a string dd/mm/yyyy a partir dos dígitos
      txtCp(Index).Text = d & "/" & m & "/" & a
   ElseIf Len(ApenasNums) > 0 Then
      ' Se não tiver 8 dígitos completos, tenta formatar parcialmente
      Select Case Len(ApenasNums)
      Case 1 To 2
         txtCp(Index).Text = ApenasNums
      Case 3 To 4
         txtCp(Index).Text = Left$(ApenasNums, 2) & "/" & Mid$(ApenasNums, 3)
      Case 5 To 7
         txtCp(Index).Text = Left$(ApenasNums, 2) & "/" & Mid$(ApenasNums, 3, 2) & "/" & Mid$(ApenasNums, 5)
      End Select
   End If
   ' Não atribuir ao txtCampo.Value aqui (evita conversões indevidas)
End Sub

'evento - quando qq tecla for digitada no campo
Private Sub opcPainel1Cp_KeyPress(Index As Integer, KeyAscii As Integer)
   opcPainel1(Index).KeyPress KeyAscii
End Sub

'evento - quando qq tecla for digitada no campo
Private Sub opcPainel1Cp_KeyDown(Index As Integer, KeyCode As Integer, Shift As Integer)
   opcPainel1(Index).KeyDown KeyCode, Shift
End Sub

'evento - quando o campo receber o foco
Private Sub opcPainel1Cp_GotFocus(Index As Integer)
   opcPainel1(Index).GotFocus
End Sub

'evento - quando o campo perder o foco
Private Sub opcPainel1Cp_LostFocus(Index As Integer)
   opcPainel1(Index).LostFocus
End Sub

'evento - quando uma opção for selecionada
Private Sub opcPainel2Cp_Click(Index As Integer)
   If vgPriVez Then Exit Sub
   If opcPainel2(Index).Locked Then
      opcPainel2(val(labopcPainel2.Caption)).Value = True
   Else
      If val(labopcPainel2.Caption) <> opcPainel2(Index).BookMark Then
         labopcPainel2.Caption = Str$(opcPainel2(Index).BookMark)
         InicializaApelidos COM_TEXTBOX
         ExecutaVisivel
         ExecutaPreValidacao
         opcPainel2(Index).Change
      End If
   End If
   opcPainel1(0).CtPri.TabStop = False
   opcPainel1(1).CtPri.TabStop = False
   opcPainel2(0).CtPri.TabStop = False
   opcPainel2(1).CtPri.TabStop = False
End Sub

'evento - quando qq tecla for digitada no campo
Private Sub opcPainel2Cp_KeyPress(Index As Integer, KeyAscii As Integer)
   opcPainel2(Index).KeyPress KeyAscii
End Sub

'evento - quando qq tecla for digitada no campo
Private Sub opcPainel2Cp_KeyDown(Index As Integer, KeyCode As Integer, Shift As Integer)
   opcPainel2(Index).KeyDown KeyCode, Shift
End Sub

'evento - quando o campo receber o foco
Private Sub opcPainel2Cp_GotFocus(Index As Integer)
   opcPainel2(Index).GotFocus
End Sub

'evento - quando o campo perder o foco
Private Sub opcPainel2Cp_LostFocus(Index As Integer)
   opcPainel2(Index).LostFocus
End Sub

'evento - inicialização do formulário
Private Sub Form_Load()
   On Error GoTo DeuErro
   Dim x As String
   Screen.MousePointer = vbHourglass
   Caption = LoadGasString(144790)
   vgFormID = 1441
   vgIdentTab = ""
   vgPriVez = True
   vgTipo = TP_RELAT
   vgCaracteristica = F_COMUM
   vgTemFiltro = False
   ReDim vgBookPg(0) As Variant, vgPodeVoltar(0) As Boolean
   Load vgFP
   With vgFP
      vgIntervalPag = ""
      Set .vgFormRel = Me
      .vgContaPg = 0
      vgWidthRel = 520 + .ScaleX(286.8078, vbMillimeters, .ScaleMode)
      vgHeightRel = 720 + .ScaleY(196.9841, vbMillimeters, .ScaleMode)
      .picAux.Width = vgWidthRel
      .picAux.Height = vgHeightRel
      .Caption = LoadGasString(7505) + Me.Caption
      .picInterna.Move 0, 0, vgWidthRel, vgHeightRel
      .picAux.ScaleMode = vbMillimeters
      .picAux.Scale (0, 0)-(286.8078, 196.9841)
      .txtFixa(0).Width = 2325
      With .txtFixa(0).Font
         .Name = "Verdana"
         .Charset = 0
         .Weight = 400
         .Size = 6
         .Underline = 0
         .Italic = 0
         .Strikethrough = 0
      End With
      Load .txtFixa(1)
      .txtFixa(1).Width = 4110
      With .txtFixa(1).Font
         .Name = "Verdana"
         .Charset = 0
         .Weight = 400
         .Size = 6
         .Underline = 0
         .Italic = 0
         .Strikethrough = 0
      End With
   End With
   vgWidthRel = vgWidthRel - 520
   vgHeightRel = vgHeightRel - 720
   vgVisivel = True
   Label(4).Caption = Printer.DeviceName
   vgSituacao = ACAO_NAVEGANDO
   Set picBox(0).Picture = LoadPicture(LoadGasPicture(136295))
   Set botConfImp.Picture = LoadPicture(LoadGasPicture(136296))
   Set botOk.Picture = LoadPicture(LoadGasPicture(136297))
   Set botCancela.Picture = LoadPicture(LoadGasPicture(136298))
   Load vgFP.picFixa(1)
   Set vgFP.picFixa(1).Picture = LoadPicture(LoadGasPicture(136299))
   Set lblAjuste = Label(6)
   vgFP.picFixa(0).AutoSize = True
   vgFP.picFixa(0).ScaleMode = vbMillimeters
   With cboSituacao
      .Clear
      .AddItem "Todos os pedidos"
      .AddItem "Somente entregues"
      .AddItem "Pedidos em fábrica"
      .ListIndex = 0
   End With
   vgQueryAtual = "SELECT [Follow Up Vendas].[Seq Follow Up], " & _
                  "[Follow Up Vendas].[Seqüência do Orçamento], " & _
                  "[Follow Up Vendas].[Data de Emissão], " & _
                  "[Follow Up Vendas].[Seqüência da Transportadora], " & _
                  "[Follow Up Vendas].[Data de Entrega], " & _
                  "[Follow Up Vendas].Dias, " & _
                  "[Follow Up Vendas].[Det 1], " & _
                  "[Follow Up Vendas].[Det 2], " & _
                  "[Follow Up Vendas].[Serie do Equipamento], " & _
                  "[Follow Up Vendas].[Descr do Material], " & _
                  "[Follow Up Vendas].[Razão Social] AS Cliente, " & _
                  "[Follow Up Vendas].Stat, " & _
                  "[Follow Up Vendas].[Venda Fechada], " & _
                  "[Follow Up Vendas].Telefone, " & _
                  "IIf(Geral.[Razão Social] Is Null, '', Geral.[Razão Social]) AS transp " & _
                  "FROM [Follow Up Vendas] LEFT JOIN Geral " & _
                  "ON [Follow Up Vendas].[Seqüência da Transportadora] = Geral.[Seqüência Do Geral]"
   Set Follow_Up_Vendas = vgDb.OpenRecordSet("SELECT TOP 1" + Mid$(vgQueryAtual, 7))
   DefineControles
   vgTooltips.Create
   Label(0).Caption = LoadGasString(144795)
   opcPainel1(0).Caption = LoadGasString(144796)
   opcPainel1(1).Caption = LoadGasString(144797)
   Label(1).Caption = LoadGasString(144798)
   opcPainel2(0).Caption = LoadGasString(144799)
   opcPainel2(1).Caption = LoadGasString(144800)
   Label(2).Caption = LoadGasString(144801)
   Label(3).Caption = LoadGasString(144802)
   vgTooltips.AddTool botConfImp, 0, LoadGasString(144803)
   vgTooltips.AddTool botOk, 0, LoadGasString(144804)
   botOk.Caption = LoadGasString(144805)
   vgTooltips.AddTool botCancela, 0, LoadGasString(144806)
   botCancela.Caption = LoadGasString(144807)
   Label(5).Caption = LoadGasString(144808)
   Label(6).Caption = LoadGasString(144809)
   vgSQL$ = Follow_Up_Vendas.Source
   AjustaTamanho Me
   Executar INICIALIZACOES
   InicializaApelidos COM_TEXTBOX
   ExecutaVisivel
   ExecutaPreValidacao
   txtCampo(1).Value = 1
   Screen.MousePointer = vbDefault
   vgUltimoTabIndex = -1                          'controla lost focus de campos optativos
   vgPriVez = False
   Exit Sub

DeuErro:
   CErr.NumErro = Err
   CErr.FunctionName = "Form_Load"
   CErr.Origem = CStr(vgFormID) + " - " + Me.Caption
   CErr.Show
End Sub


'evento - quando o formulário receber o foco
Private Sub Form_Activate()
   AtivaForm Me
End Sub

'evento - redefinido o tamanho do formulário
Private Sub Form_Resize()
   AjustaPanFundo Me
End Sub

'define as propriedades das classe dos campos do formuário
Public Sub DefineControles()
 On Error GoTo DeuErro

   Set opcPainel1(0).CtPri = opcPainel1Cp(0)
   Set opcPainel1(0).CtFdo = labopcPainel1
   opcPainel1(0).DataType = 6
   opcPainel1(0).BookMark = 0

   Set opcPainel1(1).CtPri = opcPainel1Cp(1)
   Set opcPainel1(1).CtFdo = labopcPainel1
   opcPainel1(1).DataType = 6
   opcPainel1(1).BookMark = 1

   Set opcPainel2(0).CtPri = opcPainel2Cp(0)
   Set opcPainel2(0).CtFdo = labopcPainel2
   opcPainel2(0).DataType = 6
   opcPainel2(0).BookMark = 0

   Set opcPainel2(1).CtPri = opcPainel2Cp(1)
   Set opcPainel2(1).CtFdo = labopcPainel2
   opcPainel2(1).DataType = 6
   opcPainel2(1).BookMark = 1

   Set txtCampo(0).CtPri = txtCp(0)
   txtCampo(0).DataType = 0
   txtCampo(0).Mask = ""
   txtCampo(0).BoundColumn = ""
   txtCampo(0).ListFields = ""
   txtCampo(0).OrderFields = ""
   txtCampo(0).Relation = ""
   txtCampo(0).Source = ""

   Set txtCampo(1).CtPri = txtCp(1)
   Set txtCampo(1).CtFdo = labFdo1
   Set txtCampo(1).CtScr = scrVtxtCampo1
   txtCampo(1).LargeChange = 10
   txtCampo(1).SmallChange = 1
   txtCampo(1).DataType = 3
   txtCampo(1).Mask = ""
   txtCampo(1).BoundColumn = ""
   txtCampo(1).ListFields = ""
   txtCampo(1).OrderFields = ""
   txtCampo(1).Relation = ""
   txtCampo(1).Source = ""
   txtCampo(1).Value = 1

   Set txtCampo(2).CtPri = txtCp(2)
   Set txtCampo(2).CtFdo = labFdo2
   txtCampo(2).DataType = 2
   txtCampo(2).Mask = "99/99/9999"
   txtCp(2).MaxLength = 10

   Set txtCampo(3).CtPri = txtCp(3)
   Set txtCampo(3).CtFdo = labFdo3
   txtCampo(3).DataType = 2
   txtCampo(3).Mask = "99/99/9999"
   txtCp(3).MaxLength = 10

 Exit Sub

DeuErro:
  CErr.NumErro = Err
  CErr.FunctionName = "DefineControles0"
  CErr.Origem = CStr(vgFormID) + " - " + Me.Caption
 CErr.Show
End Sub

'inicia impressão do relatório/etiqueta
Public Sub ProssegueExecucao(vgObSaida As Object, Optional ByVal vgNumPg As IMPRIME_PAGINA = 0)
   Dim vgPsm As Integer, vgPsz As Integer, vgPfn As String, vgPor As Integer, x As String, vgSemBookMark As Integer, fnum As Integer, logPath As String
   Dim i As Integer, vgLimiteInf As Single, vgSqlFiltro As String, vgTemFimRel As Boolean
   Dim vgNParc As Integer, vgNVez As Integer, vgNCop As Integer
   Dim vgEmPapel As Boolean, vgPagOk As Boolean, vgPV As Boolean, vgDifProxReg As Single, vgNumPag As Long, vgSemControlePg As Boolean, Ok As Boolean
   Dim j As Integer, m As Integer, vgQt As Integer, vgPagOrig As Integer, vgNovaPg As Integer, k As Integer, vgIniProc As Boolean, vgTestaPriCab As Boolean
   Dim vgPriQb20 As Integer, vgQuebra20 As Variant, vgPosLinMemo0 As Single, vgQdeLMemo0 As Long, vgLMemo0 As String, vgCL0 As Long, vgPula0 As Boolean, vgTF0 As Single
   Dim vgPosLinMemo1 As Single, vgQdeLMemo1 As Long, vgLMemo1 As String, vgCL1 As Long, vgPula1 As Boolean, vgTF1 As Single
   Dim Valor_Total As Double, Valor_Pago As Double
   Dim razaoSocial As String, nomeTransportadora As String, regTomadorFrete As String
   vgCL0 = 0
   vgCL1 = 0
   Screen.MousePointer = vbHourglass              'mostra ampulheta
   vgCancelou = False
   vgTemFimRel = False                            'se tem pendência para ser impressa como fim de página
   vgSemBookMark = 0
   If TypeOf vgObSaida Is Printer Then            'estamos em preview ou impressora?
      vgEmPapel = True                            'imprimindo na impressora, confirma
      GeraLogAcao LoadGasString(296) + " Controle - Pedidos Faturados"
   Else
      vgEmPapel = False
      vgObSaida.Cls
      vgFP.scrH.Value = 0                         'acerta scroll do preview
      vgFP.scrV.Value = 0
   End If
   vgDataInicial = Now                            'data e hora do inicio do relatório
   vgNovaPg = -1
   vgIniProc = False                              'flag para informar que iniciou procura em navegação (próxima, anterior, última...)

   If vgNumPg = G_IMP_ATUA_PG Then                'solicitou impressão da página atual
      vgNumPg = vgFP.vgContaPg
   End If

   vgNCop = 0                                     'controla num de cop se impressora não tem esta prop
   vgSemControlePg = (False)

OutraCopia:
   On Error GoTo DeuErro                          'vamos cercar erros...

   'abre os recordsets
   vgPV = (vgNumPg = 0 Or (vgEmPapel And vgFP.vgContaPg > 0)) 'veio do botao de prosseguir?
   If vgPV And Not (vgEmPapel And vgFP.vgContaPg > 0) Then
      If TypeOf vgObSaida Is PictureBox Then                  'estamos em preview ou impressora?
         GeraLogAcao LoadGasString(297) + " Controle - Pedidos Faturados"
      End If
      vgFiltroInicial = "" & Filtro() & ""
      If Len(vgFiltraSoUm) > 0 Then
       If Len(vgFiltroInicial) > 0 Then vgFiltroInicial = "(" + vgFiltroInicial + ") And "
         vgFiltroInicial = vgFiltroInicial + "(" + vgFiltraSoUm + ")"
      End If
   x$ = vgQueryAtual
      If Len(vgUltimoFiltro) > 0 And Len(vgFiltroInicial) > 0 Then
         x$ = InsereSQL(x$, EXP_WHERE, "(" + vgFiltroInicial + ") And (" + vgUltimoFiltro + ")")
      ElseIf Len(vgUltimoFiltro + vgFiltroInicial) > 0 Then
         x$ = InsereSQL(x$, EXP_WHERE, vgFiltroInicial + vgUltimoFiltro)
      End If
      vgOrdemInicial = ExtraiSQL(x$, EXP_ORDERBY)
      If Len(vgUltimaOrdem) > 0 Or Len(vgOrdemInicial) > 0 Then
         x$ = InsereSQL(x$, EXP_ORDERBY, vgOrdemInicial + IIf(Len(vgOrdemInicial) > 0 And Len(vgUltimaOrdem) > 0, ", ", "") + vgUltimaOrdem)
      End If
   ' Ordenação personalizada: status na sequência desejada seguido da data crescente
   On Error Resume Next
   Dim ordemStatus As String
   ordemStatus = "IIf(Stat='A Vencer',1,IIf(Stat='Vencidos',2,IIf(Stat='Aguardando Retirada',3,IIf(Stat='Entregue' Or Stat='Entregues',4,5))))"
   x$ = InsereSQL(x$, EXP_ORDERBY, ordemStatus & ", [Data de Emissão]")
   On Error GoTo DeuErro
   ' LOG: grava o SQL final em arquivo temporário para depuração (remover depois)
   On Error Resume Next
   logPath = "C:\Projetos\SistemaIrrigacao - 18.09\IRRIG\sql_debug_log.txt"
   fnum = FreeFile
   Open logPath For Append As #fnum
   Dim sIni As String, sFim As String
   sIni = "<NULL>": sFim = "<NULL>"
   If Not IsNull(Dt_Inicial) Then sIni = Format$(Dt_Inicial, "yyyy-mm-dd")
   If Not IsNull(Dt_Final) Then sFim = Format$(Dt_Final, "yyyy-mm-dd")
   Print #fnum, Now & " - Dt_Inicial=" & sIni & " Dt_Final=" & sFim & " SQL=" & x$
   Close #fnum
   ' Adicional: grava contagem/min/max para a mesma condição WHERE (diagnóstico)
   On Error Resume Next
   Dim posWhere As Integer, wherePart As String, posOrder As Integer, cntSql As String
   posWhere = InStr(1, UCase$(x$), " WHERE ")
   If posWhere > 0 Then
      wherePart = Mid$(x$, posWhere + Len(" WHERE "))
      posOrder = InStr(1, UCase$(wherePart), " ORDER BY ")
      If posOrder > 0 Then wherePart = Left$(wherePart, posOrder - 1)
      cntSql = "SELECT COUNT(*) AS Cnt, MIN([Data de Emissão]) AS MinD, MAX([Data de Emissão]) AS MaxD FROM [Follow Up Vendas] WHERE " & wherePart
      Dim tb As Object
      Set tb = vgDb.OpenRecordSet(cntSql)
      If Not tb Is Nothing Then
         Open logPath For Append As #fnum
         Print #fnum, Now & " - CNT=" & CStr(IIf(tb!Cnt Is Null, 0, tb!Cnt)) & " MinD=" & CStr(IIf(IsNull(tb!MinD), "<NULL>", tb!MinD)) & " MaxD=" & CStr(IIf(IsNull(tb!MaxD), "<NULL>", tb!MaxD))
         Close #fnum
         tb.Close
      End If
      Set tb = Nothing
   End If
   On Error GoTo DeuErro
   On Error GoTo DeuErro
   Set Follow_Up_Vendas = vgDb.OpenRecordSet(x$)
      If Follow_Up_Vendas.RecordCount = 0 And (Follow_Up_Vendas.BOF And Follow_Up_Vendas.EOF) Then
       vgPrimeiroCab = 1
       Screen.MousePointer = vbNormal
       Exit Sub
      End If
      ReDim vgBookPg(0) As Variant                            'cria variável para salvar bookmark
      ReDim vgPodeVoltar(0) As Boolean                        'controle de quebra em filhos
      GoSub IniciaImpressao
   Else
      If vgNumPg >= 0 And (vgEmPapel And vgFP.vgContaPg > 0) Then
         GoSub IniciaImpressao
      End If
      If vgModoInt = 1 Then
         vgPagOrig = vgPagRef(vgFP.vgPagRef)
      Else
         vgPagOrig = vgFP.vgPagRef
      End If
      If vgNumPg > 0 Then                                     'se estiver indo para alguma página específica
         If vgModoInt = 1 Then
            j = 0
            For i = 1 To UBound(vgPagRef)                     'vamos procurar a página solicitada no intervalo de páginas permitidas para o relatório...
               If vgPagRef(i) = vgNumPg Then
                  j = i
                  If i >= vgPagOrig Then                      'se a página encontrada estiver após a página atualamente exibida no relatório...
                     Exit For                                 'cai fora
                  End If
               End If
            Next
            If j = 0 Then
               vgFP.vgContaPg = vgFP.vgContaPg - 1            'para evitar problema na SalvaPagina
               GoTo NaoAchouPg
            End If
            vgFP.vgPagRef = j
         End If
         vgFP.vgNovaPag = vgNumPg
         If vgNumPg > UBound(vgBookPg()) Then
            vgNumPg = UBound(vgBookPg())
         End If
         vgNovaPg = vgNumPg - 1
         vgFP.vgProcuraPg = (vgNumPg <> vgFP.vgNovaPag)
      ElseIf vgNumPg = G_IMP_PROX_PG Or vgNumPg = G_IMP_ANT_PG Then
         i = (vgNumPg = G_IMP_ANT_PG) ^ (vgNumPg = G_IMP_ANT_PG) 'i=+1 para G_IMP_PROX_PG e i=-1 para G_IMP_ANT_PG
         If vgModoInt = 1 Then
            vgFP.vgProcuraPg = (vgPagRef(vgFP.vgPagRef + (i * 1)) > UBound(vgBookPg()))
            If vgFP.vgProcuraPg Then
               vgIniProc = True                                  'flag para informar que iniciamos procura
               vgFP.vgNovaPag = vgPagRef(vgFP.vgPagRef + (i * 1))
               vgNovaPg = UBound(vgBookPg) - 1
               vgFP.vgPagRef = vgFP.vgPagRef + (i * 1)
            Else
               vgNovaPg = vgPagRef(vgFP.vgPagRef + (i * 1)) - 1
            End If
         Else
            vgFP.vgProcuraPg = False
            vgNovaPg = vgFP.vgContaPg + (i * 1) - 1
         End If
      ElseIf vgNumPg = G_IMP_PRI_PG Or vgNumPg = 0 Then
         If vgEmPapel And vgFP.vgContaPg > 0 Then                'se for em papel... e tiver vindo do botão IMPRIME do preview...
            vgPagOrig = vgFP.vgPagRef
         End If
         vgFP.vgProcuraPg = False
         If vgModoInt = 1 Then
            vgNovaPg = vgPagRef(1) - 1
         Else
            vgNovaPg = 0
         End If
      ElseIf vgNumPg = G_IMP_ULT_PG Then
         If vgModoInt = 1 Then
            vgFP.vgProcuraPg = False
            If vgFP.vgUltPagVista <= vgPagRef(UBound(vgPagRef())) Then 'se ainda não visualizou a última página...
               vgNovaPg = UBound(vgBookPg()) - 1                       'vamos posicionar em uma página antes da última disponível (vai incrementar depois)
            Else
               vgNovaPg = vgPagRef(UBound(vgPagRef())) - 1             'posicionar uma página antes da última página do relatório
            End If
         Else
            If UBound(vgBookPg()) > 0 Then
               vgNovaPg = UBound(vgBookPg()) - 1
            End If
            vgFP.vgProcuraPg = True
            vgIniProc = True                                           'flag para informar que iniciamos procura
            vgFP.vgNovaPag = 0
         End If
      End If
   End If
   If vgNovaPg >= 0 Then
      If vgPodeVoltar(vgNovaPg + 1) = False And vgNovaPg > 0 Then
         If Not vgFP.vgProcuraPg Then                                  'se nao esta indo para pg ou ultima pg
            vgFP.vgProcuraPg = True
            vgIniProc = True                                           'flag para informar que iniciamos procura
            vgFP.vgNovaPag = vgNovaPg + 1
         End If
         Do While vgPodeVoltar(vgNovaPg + 1) = False                   'acerto para não deixar voltar página com quebra em filho
            vgNovaPg = vgNovaPg - 1
         Loop
         If vgModoInt = 1 Then
            If vgNumPg < 0 Then
               vgFP.vgPagRef = vgFP.vgPagRef + 1
            End If
         Else
            vgFP.vgPagRef = vgFP.vgNovaPag
         End If
      End If
      vgFP.vgContaPg = vgNovaPg
      If Len(vgBookPg(vgNovaPg + 1)) > 0 Then
         Follow_Up_Vendas.BookMark = vgBookPg(vgNovaPg + 1)
      Else
         vgSemBookMark = 1
      End If
      vgNumPag = vgNovaPg + 1
      vgCL0 = vgMemo0(vgNumPag)
      vgCL1 = vgMemo1(vgNumPag)
   Else
      vgFP.vgPagRef = 1
   End If
   vgFP.botPara.Enabled = (vgFP.vgProcuraPg Or vgNumPg = G_IMP_ULT_PG) 'habilita/desabilita stop
   vgPrimeiroCab = True

Volta:

   'Controle - Pedidos Faturados
   If Follow_Up_Vendas.RecordCount > 0 Then
      If vgNumPg = 0 And Not (vgEmPapel And vgFP.vgContaPg > 0) Then
         Follow_Up_Vendas.MoveLast
         Follow_Up_Vendas.MoveFirst
      End If
      If vgEmPapel And vgNumPg = 0 And vgPrimeiroCab Or (vgNumPg = vgPagOrig And vgEmPapel) Then
         Set vgFG = New frmGauge
         vgQReg = 0
         Load vgFG
         Set vgFG.vgFrmOrigem = Me
         vgFG.MostraGauge
      End If
      vgLimiteInf = 196
      vgDifProxReg = 0
      On Error Resume Next
      Do While Not Follow_Up_Vendas.EOF Or vgSemBookMark = 1
         vgFP.vgFimDoArq = False
         If vgPrimeiroCab Or vgPosLin > vgLimiteInf Then
            vgPosLin = vgPosLin - vgDifProxReg
            vgDifProxReg = 0
            vgPagOk = True
            vgTestaPriCab = True
            GoSub VerificaFimPg
            GoSub OutraFolha
            On Error Resume Next
         End If
         vgQuebra20 = Follow_Up_Vendas!Stat
         vgPriQb20 = 0
         Do While Not Follow_Up_Vendas.EOF
            If vgQuebra20 <> Follow_Up_Vendas!Stat Then Exit Do
            Valor_Total = MostraTotal()
            Valor_Pago = IIf(Follow_Up_Vendas![Seqüência do Orçamento] = 121088, Valor_Total, MostraPago())
            If vgPosLin > vgLimiteInf - 23.8 Or (vgPriQb20 < 2 And vgPosLin > vgLimiteInf - 27.77) Then
               vgPagOk = False
               vgPosLin = vgPosLin - vgDifProxReg
               vgDifProxReg = 0
               vgTestaPriCab = False
               GoSub VerificaFimPg
               GoSub OutraFolha
               On Error Resume Next
               vgPriQb20 = vgPriQb20 - 1
            End If
            If vgPriQb20 < 2 Then
               GoSub CabQb20
            End If
            Imprime Follow_Up_Vendas![Seqüência do Orçamento], 2.11, vgPosLin, "F=Verdana|S=6|L=10", vgObSaida, "999999", , , vgFP.vgProcuraPg
            razaoSocial = LimitaTexto(Follow_Up_Vendas!Cliente, 48)
            regTomadorFrete = NzTexto(TomadorFrete())
            nomeTransportadora = NzTexto(Follow_Up_Vendas!transp)
            If Len(regTomadorFrete) > 0 Then
               nomeTransportadora = Trim$(nomeTransportadora & IIf(Len(nomeTransportadora) > 0, " - ", "") & regTomadorFrete)
            End If
            nomeTransportadora = LimitaTexto(nomeTransportadora, 32)
            Imprime razaoSocial, 11.64, vgPosLin, "F=Microsoft Sans Serif|S=6|L=42", vgObSaida, , , , vgFP.vgProcuraPg
            Imprime nomeTransportadora, 55.29, vgPosLin, "F=Verdana|S=6|L=22", vgObSaida, , , , vgFP.vgProcuraPg
            Imprime CDate(Follow_Up_Vendas![Data de Emissão]), 79.62, vgPosLin, "F=Verdana|S=6|L=13", vgObSaida, "99/99/9999", , , vgFP.vgProcuraPg
            Imprime CDate(Follow_Up_Vendas![Data de Entrega]), 147.35, vgPosLin, "F=Verdana|S=6|L=15", vgObSaida, "99/99/9999", , , vgFP.vgProcuraPg
            Imprime Follow_Up_Vendas!Dias, 164.02, vgPosLin, "F=Verdana|S=6|L=7", vgObSaida, "9999", , , vgFP.vgProcuraPg
            Imprime Valor_Total, 110.85, vgPosLin, "F=Verdana|S=6|L=18|JD", vgObSaida, "99.999.999,99", , , vgFP.vgProcuraPg
            Imprime Valor_Pago, 128.84, vgPosLin, "F=Verdana|S=6|L=18|JD", vgObSaida, "99.999.999,99", , , vgFP.vgProcuraPg
            Imprime IIf(Follow_Up_Vendas![Seqüência do Orçamento] = 121088, 0, Valor_Total - Valor_Pago), 144.71, vgPosLin, "F=Verdana|S=6|L=18|JD", vgObSaida, "99.999.999,99", , , vgFP.vgProcuraPg
            vgPula0 = False
            vgPosLinMemo0 = 0
            vgTF0 = 0
            vgFP.txtFixa(0).Text = Follow_Up_Vendas![Det 2]
            vgQdeLMemo0 = vgFP.ContaLinhasMemo(0)
            vgPula1 = False
            vgPosLinMemo1 = 0
            vgTF1 = 0
            vgFP.txtFixa(1).Text = Follow_Up_Vendas![Descr do Material]
            vgQdeLMemo1 = vgFP.ContaLinhasMemo(1)
            Do While (vgCL0 <= vgQdeLMemo0 And (vgPosLinMemo0 + vgTF0) < (23.8 - vgTF0)) Or (vgCL1 <= vgQdeLMemo1 And (vgPosLinMemo1 + vgTF1) < (2.64 - vgTF1))
               If (vgPosLin + (vgLimiteInf - (vgLimiteInf - (vgPosLinMemo0 + vgTF0))) > vgLimiteInf) Or vgCL0 > vgQdeLMemo0 Then
                  vgPula0 = True
               End If
               If (vgPosLin + (vgLimiteInf - (vgLimiteInf - (vgPosLinMemo1 + vgTF1))) > vgLimiteInf) Or vgCL1 > vgQdeLMemo1 Then
                  vgPula1 = True
               End If
               If vgPula0 And vgPula1 Then
                  vgPagOk = False
                  vgPula0 = False
                  vgPosLinMemo0 = 0
                  vgPula1 = False
                  vgPosLinMemo1 = 0
                  vgTestaPriCab = False
                  GoSub VerificaFimPg
                  GoSub OutraFolha
                  On Error Resume Next
                  vgPriQb20 = vgPriQb20 - 1
               End If
               If vgPriQb20 < 2 Then
                  GoSub CabQb20
               End If
               If Not vgPula0 And vgCL0 <= vgQdeLMemo0 And (vgPosLinMemo0 + vgTF0) < (23.8 - vgTF0) Then
                  vgFP.txtFixa(0).Text = Follow_Up_Vendas![Det 2]
                  vgTF0 = Imprime(vgFP.ExtraiLinhaMemo(0, vgCL0), 171.69, vgPosLin + vgPosLinMemo0, "F=Verdana|S=6|L=41", vgObSaida, , , , vgFP.vgProcuraPg)
                  vgPosLinMemo0 = vgPosLinMemo0 + vgTF0
                  vgCL0 = vgCL0 + 1
               End If
               If Not vgPula1 And vgCL1 <= vgQdeLMemo1 And (vgPosLinMemo1 + vgTF1) < (2.64 - vgTF1) Then
                  vgFP.txtFixa(1).Text = Follow_Up_Vendas![Descr do Material]
                  vgTF1 = Imprime(vgFP.ExtraiLinhaMemo(1, vgCL1), 214.28, vgPosLin + vgPosLinMemo1, "F=Verdana|S=6|L=72", vgObSaida, , , , vgFP.vgProcuraPg)
                  vgPosLinMemo1 = vgPosLinMemo1 + vgTF1
                  vgCL1 = vgCL1 + 1
               End If
            Loop
            vgCL0 = 0: vgCL1 = 0
            vgPosLin = vgPosLin + Maior(vgPosLinMemo0, vgPosLinMemo1, 23.8)
            vgDifProxReg = 0
            Follow_Up_Vendas.MoveNext
            vgDifProxReg = 1
            vgPosLin = vgPosLin + vgDifProxReg
         Loop
         vgPosLin = vgPosLin - vgDifProxReg
         vgDifProxReg = 0
         vgDifProxReg = 0
         vgDifProxReg = 1
         vgPosLin = vgPosLin + vgDifProxReg
         If vgEmPapel And vgNumPg = 0 Then
            vgQReg = vgQReg + 1
            If vgModoInt = 1 Then
               If Not vgFP.vgProcuraPg Then
                  vgFG.pgb1.Value = vgFP.vgPagRef * 100 / UBound(vgPagRef())
               End If
            Else
               vgFG.pgb1.Value = vgQReg * 100 / Follow_Up_Vendas.RecordCount
            End If
            If vgFG.vgQuerCancelar Then Exit Do
         End If
      Loop
      vgPosLin = vgPosLin - vgDifProxReg
      vgDifProxReg = 0
      If vgFP.vgProcuraPg And UBound(vgBookPg) <= vgFP.vgContaPg Then               'ops... estava buscando por uma página, e ela não existe (chegou no final do arquivo)!
         If vgNumPg = G_IMP_ULT_PG Then
            vgPrimeiroCab = True
            vgFP.vgProcuraPg = True
            vgNumPg = UBound(vgBookPg())
            If Len(vgBookPg(vgNumPg)) = 0 Then     'se a última não tem bookmark, quer dizer que chegou em fim de arquivo mas tem complemento a imprimir em outra folha
               Do While Len(vgBookPg(vgNumPg)) = 0   'vamos pegar a última página com bookmark válido
                  vgNumPg = vgNumPg - 1
               Loop
               vgFP.vgContaPg = vgNumPg              'vamos voltar para última página com bookmark
               vgNumPg = G_IMP_PROX_PG               'e força navegação para próxima (que será a última)
               vgFP.vgProcuraPg = False              'sem procurar...
            End If
            GoTo OutraCopia
         Else
            vgPrimeiroCab = True
            vgFP.vgProcuraPg = False
            If vgModoInt = 1 Then
               vgPagRef(vgFP.vgPagRef) = -vgFP.vgContaPg - 1        'vamos forçar a exclusão de páginas maiores que a última no vetor de referências
               vgFP.vgContaPg = vgPagRef(vgFP.vgPagRef - 1) - 1
               j = 0
               Do While vgPagRef(vgFP.vgPagRef - 1 - j) - 1 <> 0 And vgPagRef(vgFP.vgPagRef - 1 - j) - 1 > UBound(vgBookPg) And vgFP.vgPagRef - j > 0
                  j = j + 1
                  vgFP.vgContaPg = vgPagRef(vgFP.vgPagRef - 1 - j) - 1
               Loop
               If vgFP.vgContaPg > 0 Then
                  Follow_Up_Vendas.BookMark = vgBookPg(vgFP.vgContaPg + 1)
               Else
                  vgFP.vgContaPg = 0
                  Follow_Up_Vendas.MoveFirst
               End If
            End If
            If vgModoInt = 1 Then
               GoTo Volta
             ElseIf vgFP.vgNovaPag > vgFP.vgContaPg Then
               GoSub RestauraPag
               GoTo NaoAchouPg
            End If
         End If
      ElseIf vgEmPapel And vgNumPg = 0 And vgFP.vgPagRef < UBound(vgPagRef()) Then 'estava imprimindo intervalo de página e parece ter chegado no fim do arquivo
         If Not vgFG.vgQuerCancelar Then
            vgPosLin = vgLimiteInf + 99                  'força um cabec na próxima vez
            vgFP.vgContaPg = 0                           'zera o contador de página
            GoTo Volta                                   'e vamos voltar para imprimir a próxima página
         End If
      End If
   End If

CancelouRel:
   If vgEmPapel Then
      If vgNumPg > 0 And Not vgPrimeiroCab And Not vgFG.vgQuerCancelar Then
         Unload vgFG
         Set vgFG = Nothing
         vgObSaida.EndDoc
      Else
         If vgFG.vgQuerCancelar Then
            vgCancelou = True
            vgObSaida.KillDoc
         Else
            vgObSaida.EndDoc
         End If
         If vgNumPg = 0 And vgPagOrig > 0 Then                      'solicitou impressão de todo relatório pelo preview
            vgFP.vgPagRef = vgPagOrig
            If vgModoInt = 1 Then
               vgFP.vgContaPg = vgPagRef(vgPagOrig)                     'e arruma o número da página
            Else
               vgFP.vgContaPg = vgPagOrig                               'e arruma o número da página
            End If
         End If
         vgNCop = vgNCop - 1
         If vgFG.vgQuerCancelar = False And vgNCop > 0 Then
            Unload vgFG
            Set vgFG = Nothing
            GoTo OutraCopia
         End If
      End If
      With Printer
         .ScaleMode = vgPsm
         .Orientation = vgPor
         .PaperSize = vgPsz
         .FontName = vgPfn
      End With
   Else
      GoSub SalvaPagina
   End If
   GoTo SaiDaSub

VerificaFimPg:
   If Not vgEmPapel And (Not vgTestaPriCab Or vgPrimeiroCab = False) Then
      GoSub SalvaPagina
      If Not vgFP.vgProcuraPg Then                      'se não está procurando por uma página
         If vgFP.vgContaPg <> vgFP.vgNovaPag Then                   'opa... parece que o usuário precionou o STOP
            If vgNumPg = G_IMP_ANT_PG Or vgNumPg = G_IMP_PROX_PG Then 'se estava movendo para próxima página ou anterior
               vgFP.vgContaPg = vgPagOrig                               'vamos restaurar a página original
               If vgNumPg = G_IMP_ANT_PG Then                           'restaura o contador
                  vgFP.vgPagRef = vgFP.vgPagRef + 1
               Else
                  vgFP.vgPagRef = vgFP.vgPagRef - 1
               End If
               GoSub SalvaPagina                    'ajusta a botoeira do preview novamente
            End If
         End If
         GoTo SaiDaSub                          'cai fora
      End If
   ElseIf vgEmPapel And vgNumPg > 0 And Not vgPrimeiroCab Then
      If vgFP.vgContaPg = vgFP.vgNovaPag Then GoTo CancelouRel
   ElseIf vgNumPg = 0 And Not vgPrimeiroCab Then
      If Not vgFP.vgProcuraPg Then
         If vgModoInt = 1 And vgFP.vgPagRef >= UBound(vgPagRef()) Then         'se estava imprimindo intervalo de páginas...
            GoTo CancelouRel                                                     'se chegou à última página do intervalo, cai fora
         End If
         GoSub SalvaInicioPg
      End If
   End If
   Return

OutraFolha:
   If vgEmPapel = False Then
      GoSub SalvaInicioPg
      If vgFP.vgProcuraPg And vgNumPg = G_IMP_ULT_PG Then
         vgFP.txtPag.Text = Trim$(Str$(vgFP.vgContaPg + 1))
         vgFP.txtPag.Refresh
      End If
   ElseIf Not vgSemControlePg Then
      If Not vgPrimeiroCab And Not vgFP.vgProcuraPg Then vgObSaida.NewPage
   End If
   vgFP.vgContaPg = vgFP.vgContaPg + 1
   If vgFP.vgProcuraPg = False Then
    If vgModoInt = 0 Then
     vgFP.vgNovaPag = vgFP.vgContaPg
    Else
     If vgPagRef(vgFP.vgPagRef) < 0 Then
      GoSub ArrumaVetor
     Else
      If vgNumPg = G_IMP_PROX_PG Or vgNumPg > vgFP.vgContaPg Or (vgNumPg = 0 And vgModoInt = 0) Then
       If Not vgIniProc Then                  'se não estava procurando...
        vgIniProc = True                      'agora seta flag de procura
        vgFP.vgPagRef = vgFP.vgPagRef + 1
       Else                                   'usuário cancelou busca pela próxima página
        vgFP.vgPagRef = vgFP.vgPagRef - 1
       End If
      ElseIf vgNumPg = G_IMP_ANT_PG Or (vgNumPg > 0 And vgNumPg < vgFP.vgContaPg) Then
       If Not vgIniProc Then                  'se não estava procurando...
        vgIniProc = True                      'agora seta flag de procura
        vgFP.vgPagRef = vgFP.vgPagRef - 1
       Else                                   'usuário cancelou busca pela página anterior
        vgFP.vgPagRef = vgFP.vgPagRef + 1
       End If
      ElseIf vgNumPg = G_IMP_PRI_PG Or vgNumPg = 0 Then
       If vgPrimeiroCab Then
        vgFP.vgPagRef = 1
       Else
        vgFP.vgPagRef = vgFP.vgPagRef + 1
       End If
      ElseIf vgNumPg = G_IMP_ULT_PG Then
       If Not vgIniProc Then                  'se não estava procurando...
        vgIniProc = True                      'agora seta flag de procura
        vgFP.vgPagRef = UBound(vgPagRef())
       Else
        'usuário cancelou busca para última página, vamos procurar o último bookmark existente no intervalo de páginas especificado
        For j = UBound(vgPagRef()) To 1 Step -1           'vamos correr as páginas de referência, de trás para frente
         If vgPagRef(j) <= UBound(vgBookPg) Then         'se tem bookmark para a página correspondente
          vgFP.vgPagRef = j                               'vamos utilizar essa página
          Exit For
         End If
        Next
       End If
      End If
     End If
     vgFP.vgNovaPag = vgPagRef(vgFP.vgPagRef)             'pega o número de página real referente ao número da página no intervalo
     If vgFP.vgNovaPag = 0 Then GoTo CancelouRel          'ops... vamos sair do relatório
     If vgFP.vgNovaPag <> vgFP.vgContaPg Then             'se a página atual não for a página procurada
      If vgFP.vgNovaPag <= UBound(vgBookPg()) Then       'se tem bookmark para a página procurada
       vgFP.vgContaPg = vgFP.vgNovaPag                    'vamos posicionar diretamente na nova página
       Do While vgPodeVoltar(vgFP.vgContaPg) = False      'acerto para não deixar voltar página com quebra em filho
        vgFP.vgContaPg = vgFP.vgContaPg - 1
       Loop
       vgFP.vgProcuraPg = (vgFP.vgContaPg <> vgFP.vgNovaPag)
       Follow_Up_Vendas.BookMark = vgBookPg(vgFP.vgContaPg)      'seta o bookmark correto
       If Not vgEmPapel Then
        vgFP.picAux.Cls                                    'limpa a área
        vgFP.picAux.Picture = LoadPicture("")              'para imprimir a página
       End If
      Else                                                'não tem bookmark...
       vgFP.vgProcuraPg = True                            'então vamos procurar
      End If
     Else
      If Not vgEmPapel Then
       vgFP.picAux.Cls                                     'limpa a área
       vgFP.picAux.Picture = LoadPicture("")               'para imprimir a página
      End If
     End If
    End If
   ElseIf vgFP.vgNovaPag = vgFP.vgContaPg Then            'se a página atual é a página procurada
    vgFP.vgProcuraPg = False                              'vamos cancelar a procura!
   End If
   If vgModoInt = 0 Then                                  'se não tem intervalo de páginas especificado
    vgNumPag = vgFP.vgContaPg
    vgFP.vgPagRef = vgFP.vgContaPg                      'com o mesmo número para página de referência
   Else                                                   'senão... tem intervalo de página
    vgNumPag = vgFP.vgNovaPag                             'então pela o número da nova página
   End If
   vgPosLin = 41.26
   Imprime "", 0, 41.13, "LIN=B0/" + Trim(Str(0)) + "/" + Trim$(Str$(41.13)) + "/" + Trim(Str(286.77)) + "/" + Trim$(Str$(41.13)), vgObSaida, , , , vgFP.vgProcuraPg
   Imprime vgFP.picFixa(1), 1.32, 2.64, "FIG=FigFixa1.jpg/38.88/31.21", vgObSaida, , , , vgFP.vgProcuraPg
   Imprime "IRRIGAÇÃO PENÁPOLIS", 42.59, 10.84, "F=Verdana|W=700|S=15.75|L=118", vgObSaida, , , , vgFP.vgProcuraPg
   Imprime "Controle - Pedidos Faturados", 42.59, 17.98, "F=Verdana|W=700|S=15.75|L=111", vgObSaida, , , , vgFP.vgProcuraPg
   If Not Vazio(Dt_Inicial) And Not Vazio(Dt_Final) Then
      ' Espaçamento aproximadamente 1 cm abaixo do subtítulo (Controle - Pedidos Faturados)
      Imprime "De: " & Dt_Inicial & " a " & Dt_Final, 55.06, 28.0, "F=Courier New|W=700|S=14.25", vgObSaida, , , , vgFP.vgProcuraPg
   End If
   Imprime "Pedido", 2.11, 38.09, "F=Verdana|W=700|S=6.75|L=11", vgObSaida, , , , vgFP.vgProcuraPg
   Imprime "Razão Social", 11.9, 38.09, "F=Verdana|W=700|S=6.75|L=44", vgObSaida, , , , vgFP.vgProcuraPg
   Imprime "Transportadora", 55.29, 38.09, "F=Verdana|W=700|S=6.75|L=28", vgObSaida, , , , vgFP.vgProcuraPg
   Imprime "Dt. Pedido", 79.62, 38.09, "F=Verdana|W=700|S=6.75|L=16", vgObSaida, , , , vgFP.vgProcuraPg
   Imprime "Dt. Entrega", 147.35, 38.09, "F=Verdana|W=700|S=6.75|L=19", vgObSaida, , , , vgFP.vgProcuraPg
   Imprime "Dias", 164.28, 38.09, "F=Verdana|W=700|S=6.75|L=11", vgObSaida, , , , vgFP.vgProcuraPg
   Imprime "Decrição Mat.", 215.07, 38.35, "F=Verdana|W=700|S=6.75|L=54", vgObSaida, , , , vgFP.vgProcuraPg
   Imprime CDate(vgDataInicial), 247.88, 4.76, "F=Courier New|S=11.25|L=25", vgObSaida, "99/99/9999", , , vgFP.vgProcuraPg
   Imprime CDate(vgDataInicial), 274.33, 4.76, "F=Courier New|S=11.25|L=12", vgObSaida, "99:99", , , vgFP.vgProcuraPg
   Imprime "Página:", 273.82, 0, "F=Courier New|S=11.25|L=18|JD", vgObSaida, , , , vgFP.vgProcuraPg
   Imprime vgNumPag, 286.39, 0, "F=Courier New|S=11.25|L=11|JD", vgObSaida, "9999", , , vgFP.vgProcuraPg
   Imprime "Total", 103.96, 38.09, "F=Verdana|W=700|S=6.75|L=10", vgObSaida, , , , vgFP.vgProcuraPg
   Imprime "Valor", 103.96, 35.18, "F=Verdana|W=700|S=6.75|L=9", vgObSaida, , , , vgFP.vgProcuraPg
   Imprime "Valor", 118.78, 35.18, "F=Verdana|W=700|S=6.75|L=8", vgObSaida, , , , vgFP.vgProcuraPg
   Imprime "Recebido", 116.66, 38.09, "F=Verdana|W=700|S=6.75|L=12", vgObSaida, , , , vgFP.vgProcuraPg
   Imprime "Valor", 134.65, 35.18, "F=Verdana|W=700|S=6.75|L=8", vgObSaida, , , , vgFP.vgProcuraPg
   Imprime "Restante", 132.53, 38.09, "F=Verdana|W=700|S=6.75|L=16", vgObSaida, , , , vgFP.vgProcuraPg
   Imprime "Impresso por: ->" & " " & vgPWUsuario, 286.39, 9.52, "F=Courier New|I=-1|L=61|JD", vgObSaida, , , , vgFP.vgProcuraPg
   Imprime "Obs", 171.69, 38.09, "F=Verdana|W=700|S=6.75|L=11", vgObSaida, , , , vgFP.vgProcuraPg
   vgPrimeiroCab = False
   vgTemFimRel = False
   Return

CabQb20:
   If vgPriQb20 < 1 Then
      Dim statusAtual As String
      Dim statusTitulo As String
      Dim corStatus As String
      statusAtual = Trim$(Follow_Up_Vendas!Stat & "")
      corStatus = CorDeStatus(statusAtual)
      If Len(corStatus) > 0 Then
         Dim topoFaixa As Single
         Dim alturaFaixa As Single
         topoFaixa = vgPosLin
         alturaFaixa = 3.43
         Select Case UCase$(statusAtual)
            Case "VENCIDOS"
               topoFaixa = vgPosLin
               alturaFaixa = 3.43
            Case "A VENCER", "AGUARDANDO RETIRADA", "ENTREGUE", "ENTREGUES"
               topoFaixa = vgPosLin + 0.26
               alturaFaixa = 3.17
            Case Else
               topoFaixa = vgPosLin + 0.26
               alturaFaixa = 3.17
         End Select
         Imprime "", 1.32, topoFaixa, "L=285|BOX=" & corStatus & "/1.32/" + Str$(topoFaixa) + "/284.65/" + Str$(alturaFaixa), vgObSaida, , , , vgFP.vgProcuraPg
      End If
      If UCase$(statusAtual) = "ENTREGUE" Or UCase$(statusAtual) = "ENTREGUES" Then
         statusTitulo = "Pedidos Entregues"
      Else
         statusTitulo = statusAtual
      End If
      Imprime statusTitulo, 2.11, vgPosLin, "F=Verdana|W=700|S=6.75|L=142", vgObSaida, , , , vgFP.vgProcuraPg
      vgPosLin = vgPosLin + 3.96
      vgDifProxReg = 0
   End If
   vgPriQb20 = 2
   Return

SalvaInicioPg:
   If (vgPV Or vgFP.vgContaPg + 1 > UBound(vgBookPg)) And (vgTemFimRel Or Not Follow_Up_Vendas.EOF) Then 'testa se precisa mais variáveis
      If vgFP.vgContaPg + 1 > UBound(vgBookPg) Then
         ReDim Preserve vgBookPg(vgFP.vgContaPg + 1) As Variant                                          'cria variável para salvar bookmark
         ReDim Preserve vgPodeVoltar(vgFP.vgContaPg + 1) As Boolean                                      'controle de quebra em filhos
         ReDim Preserve vgMemo0(vgFP.vgContaPg + 1) As Long
         ReDim Preserve vgMemo1(vgFP.vgContaPg + 1) As Long
         vgMemo0(vgFP.vgContaPg + 1) = vgCL0
         vgMemo1(vgFP.vgContaPg + 1) = vgCL1
      End If
      If Not Follow_Up_Vendas.EOF Then
         vgBookPg(vgFP.vgContaPg + 1) = Follow_Up_Vendas.BookMark                                        'salva inicio desta página
      Else
         vgBookPg(vgFP.vgContaPg + 1) = ""
         vgPodeVoltar(vgFP.vgContaPg + 1) = True   'pode voltar preview para esta página?
      End If
      vgPodeVoltar(vgFP.vgContaPg + 1) = vgPagOk Or vgPrimeiroCab 'pode voltar preview para esta página?
   End If
   If Not vgPriVez Then
      vgPagOk = False
   End If
   Return

SalvaPagina:
   If Not vgEmPapel Or (vgEmPapel And vgNumPg = 0) Then
      With vgFP
         If .vgFimDoArq = False And Follow_Up_Vendas.EOF Then
            .vgFimDoArq = True
         End If
         Ok = ((((vgTemFimRel Or .vgFimDoArq = False) Or .vgContaPg < .vgUltPagVista) And vgModoInt = 0) Or ((.vgPagRef < UBound(vgPagRef())) And vgModoInt = 1)) And Not .vgProcuraPg
         If Ok <> .botMais.Enabled Then
            .botMais.Enabled = Ok                                 'habilita/desabilita botões do preview
         End If
         Ok = .botMais.Enabled And Not .vgProcuraPg
         If Ok <> .botUltimo.Enabled Then
            .botUltimo.Enabled = Ok
         End If
         Ok = (.vgPagRef > 1 And Not .vgProcuraPg)
         If Ok <> .botVolta.Enabled Then
            .botVolta.Enabled = Ok
         End If
         Ok = (.vgPagRef > 1 And Not .vgProcuraPg)
         If Ok <> .botPrimeiro.Enabled Then
            .botPrimeiro.Enabled = Ok
         End If
         GoSub SalvaInicioPg
         If .vgProcuraPg Or (.vgContaPg <> .vgNovaPag And .vgNovaPag <> 0) Then 'procurando pg adiante...
            If .vgProcuraPg And ((vgModoInt = 0 And (Not vgTemFimRel And Follow_Up_Vendas.EOF)) Or .vgContaPg = .vgNovaPag) Then
               .ShowPreview                       'mostra preview
            Else
               .picAux.Cls                        'limpa a área para a próxima pg
               vgPosLin = vgLimiteInf + 99        'força um cabec na próxima vez
            End If
         Else
            .ShowPreview
         End If
      End With
   End If
   Return
'retira as páginas inválidas do intervalo especificado!
ArrumaVetor:
   vgQt = 0
   j = 0
   k = 0
   For i = 1 To UBound(vgPagRef())
      If vgPagRef(i) < 0 Or (Abs(vgPagRef(i)) >= k And k > 0) And i < UBound(vgPagRef()) - vgQt + 1 Then
         If vgPagRef(i) < k And vgPagRef(i) <> 0 Then k = Abs(vgPagRef(i))
         For j = 1 To UBound(vgPagRef())
            If Abs(vgPagRef(j)) >= k And j <= UBound(vgPagRef()) - vgQt + 1 Then
               vgQt = vgQt + 1
               For m = j + 1 To UBound(vgPagRef())
                  vgPagRef(m - 1) = vgPagRef(m)
               Next
               If j <= UBound(vgPagRef()) - vgQt Then
                  j = j - 1
               End If
            End If
         Next
         If i < UBound(vgPagRef()) Then
            i = i - 1
         End If
      End If
   Next
   ReDim Preserve vgPagRef(UBound(vgPagRef()) - vgQt) As Integer
   
   If vgFP.vgPagRef > UBound(vgPagRef()) Then
      vgFP.vgPagRef = UBound(vgPagRef())
   End If
   Return

RestauraPag:
   vgFP.txtPag.Text = CStr(vgPagOrig)
   vgFP.botPara.Enabled = False
   vgFP.vgProcuraPg = False
   If vgPagOrig < vgFP.vgContaPg Then             'se não for a última página, vamos restaurar o bookmark para atualizar os botões do preview
      Follow_Up_Vendas.BookMark = vgBookPg(vgPagOrig)
   End If
   vgFP.vgContaPg = vgPagOrig
   vgFP.vgPagRef = vgPagOrig
   Return

IniciaImpressao:
   On Error Resume Next                           'algumas impressoras não tem certas propriedades
   If vgEmPapel Then
      With Printer                                'salva configs da impressora
         vgPsm = .ScaleMode
         vgPor = .Orientation
         vgPsz = .PaperSize
         vgPfn = .FontName
         .PaperSize = vbPRPSA4
         .Orientation = vbPRORLandscape
         If Err Then Err.Clear
         If vgNumPg <> 0 Then                     'procurando pg para imprimir
            .Copies = 1
         Else
            .Copies = vgCopias                    'número de cópias configurada no diálogo
            If Err And vgNCop = 0 Then vgNCop = vgCopias
         End If
      End With
   End If
   With vgObSaida.Font
      'ajusta para nós...
      .Name = "MS Sans Serif"
      .Charset = 0
      .Weight = 400
      .Size = 8.25
      .Underline = 0
      .Italic = 0
      .Strikethrough = 0
      vgObSaida.ForeColor = &H0
      vgObSaida.ScaleMode = vbMillimeters         'vamos trabalhar em mm
   End With
   On Error GoTo DeuErro                          'vamos cercar erros...
   Return

NaoAchouPg:
   GoSub SalvaPagina
   vgFP.txtPag.Text = CStr(vgPagOrig)
   MsgBox "A página especificada não foi encontrada no relatório!", vbExclamation
   GoTo FimDaSub

DeuErro:                                          'deu um erro
   Select Case Err                                'vamos verificar se deu algum erro
      Case 3197                                   'registro acabou de ser alterado por outro usuário
         Resume
      Case 3046, 3158, 3186, 3187, 3188, 3189, 3218, 3260
         vgNVez = vgNVez + 1                      'outro usuário acessando
         If vgNVez <= 10 Then                     'vamos tentar 10 vezes
            Delay 0.5                             'mais antes espera 1/2 segundo
            Resume                                'antes de tentar
         End If
   End Select
   If Err <> 65524 Then                           'se não for um erro de crítica
      CErr.NumErro = Err
      CErr.Show
   End If
   vgPrimeiroCab = 2                              'flag para não dar a msg novamente

SaiDaSub:                                         'saída comum da sub...

FimDaSub:
   If Not vgFG Is Nothing Then
      Unload vgFG
      Set vgFG = Nothing
   End If
   Err.Clear
   Screen.MousePointer = vbDefault                'ponteiro do mouse = default
End Sub

'evento - configura impressora
Private Sub botConfImp_Click()
   Dim x As String
   ConfigImpressora                               'chama diáglogo de configuração impressora
   x$ = Printer.DeviceName
   Label(4).Caption = x$
End Sub

'evento - valida digitações e prossegue a execução
Private Sub botOk_Click()
   Dim k As String
   If vgPriVez Then Exit Sub
   vgPriVez = True
   k$ = Executar(VALIDACOES)                      'faz validaçao dos campos
   If Len(k$) > 0 Then                            'EPA!... tem campo errado
      If InStr(k$, "|") > 0 Then
         Err.Source = Parse$(k$, "|")
         Err.Number = val(Parse$(k$, "|"))        'deu erro
         Err.Description = Parse$(k$, "|")
         CErr.NumErro = Err.Number
         CErr.Show
      Else
         Beep                                     'manda aviso sonoro
         MsgBox k$, vbCritical, vgAtencao$        'mostra mensagem do erro
      End If
      Screen.MousePointer = vbDefault             'reseta o pointer do mouse
      vgPriVez = False
      Exit Sub                                    'e sai para continuar a ediçao ou inclusao
   End If                                         'OK. os campos estao certos
   vgFP.vgFimDoArq = False                        'é fim do arquivo
   vgFP.vgContaPg = 0                             'contador de paginas
   vgFP.vgNovaPag = 0                             'número da página real dentro do intervalo (Ex: 3;5;1;2 - se estiver na página real 5, a página atual será 2)
   vgFP.vgPagRef = 0                              'número da página de referência
   vgFP.vgUltPagVista = 0                         'ate onde ja fomos...
   If vgDestRel = 0 Then                          'onde vai ser gerado (janela ou papel)
      vgFP.cboEscala.ListIndex = 3                'escal 100%
      ProssegueExecucao vgFP.picAux               'faz preview
   Else
      vgFP.vgContaPg = 0
      ProssegueExecucao Printer                   'joga direto na impressora
   End If
   If vgPrimeiroCab <> 2 Then                     'se não deu erro
      If vgPrimeiroCab Then                       'nenhuma página foi impressa
         MsgBox LoadGasString(92), vbExclamation, vgAtencao$ 'apresenta a mensagem
      ElseIf vgDestRel = 0 Then
         vgFP.ShowPreview                                    'arruma form preview
         vgFP.Visible = True                                 'mostra form de preview
         Visible = False                                     'esconde o este form
         vgVisivel = False                                   'e seta variavel
      End If
   End If
   vgPriVez = False
End Sub

'evento - cancela formulário
Private Sub botCancela_Click()
   Unload Me
End Sub

'evento - antes de descarregar o formulário
Private Sub Form_QueryUnload(Cancel As Integer, UnloadMode As Integer)
   Cancel = Cancel Or (Not vgFG Is Nothing)
End Sub

'evento - descarregando o formulário da memória
Private Sub Form_Unload(Cancel As Integer)
   Dim i As Integer
   On Error Resume Next
   FinalizaForm Me
      Unload vgFP.txtFixa(1)
   Unload vgFP.picFixa(1)
   Set lblAjuste = Nothing
   For i = 0 To UBound(txtCampo)
      txtCampo(i).Finalize
      Set txtCampo(i) = Nothing
   Next
   If Not Follow_Up_Vendas Is Nothing Then
      Follow_Up_Vendas.CloseRecordset
      Set Follow_Up_Vendas = Nothing
   End If
   Set vgFG = Nothing
   Set vgTooltips = Nothing
   Set vgFP.vgFormRel = Nothing                   'variável associada a este form
   Unload vgFP                                    'descarrega o form de preview
   Set vgFP = Nothing                             'tira o form de preview da memória
   Set frmRelFollo = Nothing                      'libera o segmento de código do form
   If vgRelAtivo = vgFormID Then vgRelAtivo = 0
End Sub

'evento - quando qq tecla for digitada no formulário
Private Sub Form_KeyPress(KeyAscii As Integer)
   If KeyAscii = vbKeyEscape Then                 'se teclou ESC
      Unload Me                                   'tira este form da memória
   End If
End Sub


'evento - quando o valor da rolagem modificar
Private Sub scrVtxtCampo1_Change()
   txtCampo(1).ScrChange
End Sub

'evento - quando o mouse for pressionado sobre o campo
Private Sub txtCp_MouseDown(Index As Integer, Button As Integer, Shift As Integer, x As Single, y As Single)
   txtCampo(Index).MouseDown
End Sub



