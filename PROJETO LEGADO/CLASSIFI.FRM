VERSION 5.00
Begin VB.Form frmClassifi 
   ClientHeight    =   4800
   ClientLeft      =   600
   ClientTop       =   2115
   ClientWidth     =   9840
   ForeColor       =   &H80000008&
   KeyPreview      =   -1  'True
   LinkTopic       =   "Classifi"
   LockControls    =   -1  'True
   MDIChild        =   -1  'True
   ScaleHeight     =   4800
   ScaleWidth      =   9840
   Begin IRRIG.GPainel Painel 
      Height          =   4785
      Index           =   0
      Left            =   15
      TabIndex        =   21
      TabStop         =   0   'False
      Top             =   15
      Width           =   9810
      _ExtentX        =   17304
      _ExtentY        =   8440
      BevelOuter      =   0
      Stretch         =   -1  'True
      Begin VB.PictureBox picBox 
         BorderStyle     =   0  'None
         ForeColor       =   &H80000008&
         Height          =   1260
         Index           =   0
         Left            =   75
         ScaleHeight     =   1260
         ScaleWidth      =   1350
         TabIndex        =   22
         Top             =   60
         Width           =   1350
      End
      Begin VB.TextBox txtCp 
         Alignment       =   1  'Right Justify
         Appearance      =   0  'Flat
         BackColor       =   &H00E0E0E0&
         DataField       =   "Seqüência da Classificação"
         BeginProperty Font 
            Name            =   "Microsoft Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   300
         Index           =   0
         Left            =   645
         Locked          =   -1  'True
         MultiLine       =   -1  'True
         TabIndex        =   0
         TabStop         =   0   'False
         Top             =   1485
         Width           =   690
      End
      Begin VB.TextBox txtCp 
         Alignment       =   1  'Right Justify
         Appearance      =   0  'Flat
         DataField       =   "NCM"
         BeginProperty Font 
            Name            =   "Microsoft Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   300
         Index           =   1
         Left            =   2145
         MultiLine       =   -1  'True
         TabIndex        =   1
         Top             =   1485
         Width           =   1095
      End
      Begin VB.CheckBox ChkCp 
         Caption         =   "Inativo"
         DataField       =   "Inativo"
         BeginProperty Font 
            Name            =   "Microsoft Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00000080&
         Height          =   240
         Index           =   0
         Left            =   8760
         TabIndex        =   7
         TabStop         =   0   'False
         Top             =   285
         Width           =   885
      End
      Begin VB.TextBox txtCp 
         Appearance      =   0  'Flat
         DataField       =   "Descrição do NCM"
         BeginProperty Font 
            Name            =   "Microsoft Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   300
         Index           =   2
         Left            =   4530
         MaxLength       =   100
         TabIndex        =   2
         Top             =   1485
         Width           =   5115
      End
      Begin VB.TextBox txtCp 
         Alignment       =   1  'Right Justify
         Appearance      =   0  'Flat
         DataField       =   "Porcentagem do IPI"
         BeginProperty Font 
            Name            =   "Microsoft Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   300
         Index           =   3
         Left            =   645
         MultiLine       =   -1  'True
         TabIndex        =   3
         Top             =   1950
         Width           =   690
      End
      Begin VB.CheckBox ChkCp 
         Caption         =   "Produto Diferido"
         DataField       =   "Produto Diferido"
         BeginProperty Font 
            Name            =   "Microsoft Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00000080&
         Height          =   240
         Index           =   1
         Left            =   1590
         TabIndex        =   8
         TabStop         =   0   'False
         Top             =   1080
         Width           =   1755
      End
      Begin VB.CheckBox ChkCp 
         Caption         =   "Redução de B.C."
         DataField       =   "Redução de Base de Cálculo"
         BeginProperty Font 
            Name            =   "Microsoft Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00000080&
         Height          =   240
         Index           =   2
         Left            =   3420
         TabIndex        =   9
         TabStop         =   0   'False
         Top             =   1080
         Width           =   1830
      End
      Begin VB.TextBox txtCp 
         Alignment       =   1  'Right Justify
         Appearance      =   0  'Flat
         DataField       =   "IVA"
         BeginProperty Font 
            Name            =   "Microsoft Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   300
         Index           =   4
         Left            =   2145
         MultiLine       =   -1  'True
         TabIndex        =   4
         Top             =   1935
         Width           =   1095
      End
      Begin IRRIG.GPainel Painel 
         Height          =   240
         Index           =   1
         Left            =   6390
         TabIndex        =   32
         TabStop         =   0   'False
         Top             =   1995
         Width           =   900
         _ExtentX        =   1588
         _ExtentY        =   423
         BevelOuter      =   0
         BevelWidth      =   0
         BorderWidth     =   0
         Stretch         =   -1  'True
         Begin VB.OptionButton opcPainel1Cp 
            Caption         =   "1"
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
            TabIndex        =   5
            Top             =   0
            Width           =   390
         End
         Begin VB.OptionButton opcPainel1Cp 
            Caption         =   "2"
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
            Left            =   465
            TabIndex        =   6
            Top             =   0
            Width           =   390
         End
         Begin VB.Label labopcPainel1 
            BorderStyle     =   1  'Fixed Single
            DataField       =   "Anexo da Redução"
            Height          =   120
            Left            =   150
            TabIndex        =   33
            Top             =   180
            Visible         =   0   'False
            Width           =   240
         End
      End
      Begin VB.OptionButton opcPainel0Cp 
         Caption         =   "7"
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
         Height          =   255
         Index           =   2
         Left            =   7935
         TabIndex        =   12
         Top             =   1995
         Width           =   495
      End
      Begin VB.OptionButton opcPainel0Cp 
         Caption         =   "12"
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
         Height          =   255
         Index           =   0
         Left            =   8475
         TabIndex        =   13
         Top             =   1995
         Width           =   555
      End
      Begin VB.CheckBox ChkCp 
         Caption         =   "Tem Convênio"
         DataField       =   "Tem Convênio"
         BeginProperty Font 
            Name            =   "Microsoft Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00000080&
         Height          =   255
         Index           =   3
         Left            =   5325
         TabIndex        =   10
         TabStop         =   0   'False
         Top             =   1080
         Width           =   1635
      End
      Begin VB.TextBox txtCp 
         Appearance      =   0  'Flat
         DataField       =   "Cest"
         BeginProperty Font 
            Name            =   "Microsoft Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   300
         Index           =   5
         Left            =   4530
         MaxLength       =   7
         TabIndex        =   11
         Top             =   1935
         Width           =   1020
      End
      Begin VB.OptionButton opcPainel0Cp 
         Caption         =   "18"
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
         Height          =   255
         Index           =   1
         Left            =   9135
         TabIndex        =   14
         Top             =   1995
         Width           =   675
      End
      Begin VB.TextBox txtCp 
         Appearance      =   0  'Flat
         DataField       =   "Un Exterior"
         BeginProperty Font 
            Name            =   "Microsoft Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   300
         Index           =   6
         Left            =   4530
         MaxLength       =   10
         TabIndex        =   15
         Top             =   2355
         Width           =   1020
      End
      Begin VB.TextBox txtCp 
         Alignment       =   1  'Right Justify
         Appearance      =   0  'Flat
         BeginProperty Font 
            Name            =   "Microsoft Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   300
         Index           =   7
         Left            =   1050
         MaxLength       =   6
         TabIndex        =   16
         Top             =   2700
         Width           =   900
      End
      Begin VB.CommandButton bottxtCampo7 
         Caption         =   "P"
         Height          =   300
         Index           =   1
         Left            =   1980
         TabIndex        =   17
         TabStop         =   0   'False
         Top             =   2700
         Width           =   300
      End
      Begin VB.TextBox txtReducaoIBS 
         Alignment       =   1  'Right Justify
         Appearance      =   0  'Flat
         BackColor       =   &H00E0E0E0&
         BeginProperty Font 
            Name            =   "Microsoft Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   300
         Left            =   1050
         Locked          =   -1  'True
         TabIndex        =   18
         TabStop         =   0   'False
         Top             =   3050
         Width           =   1200
      End
      Begin VB.TextBox txtReducaoCBS 
         Alignment       =   1  'Right Justify
         Appearance      =   0  'Flat
         BackColor       =   &H00E0E0E0&
         BeginProperty Font 
            Name            =   "Microsoft Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   300
         Left            =   3150
         Locked          =   -1  'True
         TabIndex        =   19
         TabStop         =   0   'False
         Top             =   3050
         Width           =   1200
      End
      Begin VB.TextBox txtTipoAliquota 
         Appearance      =   0  'Flat
         BackColor       =   &H00E0E0E0&
         BeginProperty Font 
            Name            =   "Microsoft Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   300
         Left            =   5370
         Locked          =   -1  'True
         TabIndex        =   20
         TabStop         =   0   'False
         Top             =   3050
         Width           =   4230
      End
      Begin VB.Label Label 
         Alignment       =   1  'Right Justify
         AutoSize        =   -1  'True
         Caption         =   "*NCM:"
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
         Index           =   0
         Left            =   1500
         TabIndex        =   23
         Top             =   1515
         Width           =   570
      End
      Begin VB.Label Label 
         Alignment       =   2  'Center
         AutoSize        =   -1  'True
         Caption         =   "Classificação Fiscal (NCM)"
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
         Index           =   1
         Left            =   1590
         TabIndex        =   24
         Top             =   465
         Width           =   5325
      End
      Begin VB.Label Label 
         AutoSize        =   -1  'True
         Caption         =   "-"
         ForeColor       =   &H80000008&
         Height          =   195
         Index           =   2
         Left            =   9720
         TabIndex        =   25
         Top             =   2790
         Width           =   45
      End
      Begin VB.Label Label 
         AutoSize        =   -1  'True
         Caption         =   "Seq.:"
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
         Left            =   105
         TabIndex        =   26
         Top             =   1515
         Width           =   465
      End
      Begin VB.Label Label 
         AutoSize        =   -1  'True
         Caption         =   "*Descrição:"
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
         Index           =   4
         Left            =   3405
         TabIndex        =   27
         Top             =   1515
         Width           =   1050
      End
      Begin VB.Label Label 
         Alignment       =   1  'Right Justify
         AutoSize        =   -1  'True
         Caption         =   "% IPI:"
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
         Index           =   5
         Left            =   75
         TabIndex        =   28
         Top             =   1980
         Width           =   495
      End
      Begin VB.Label Label 
         Alignment       =   1  'Right Justify
         AutoSize        =   -1  'True
         Caption         =   "IVA:"
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
         Index           =   6
         Left            =   1710
         TabIndex        =   29
         Top             =   1980
         Width           =   360
      End
      Begin VB.Label Label 
         AutoSize        =   -1  'True
         Caption         =   "Anexo:"
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
         Left            =   5700
         TabIndex        =   30
         Top             =   1980
         Width           =   615
      End
      Begin VB.Label Label 
         AutoSize        =   -1  'True
         Caption         =   "Alíq:"
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
         Index           =   8
         Left            =   7410
         TabIndex        =   31
         Top             =   1980
         Width           =   390
      End
      Begin VB.Label labopcPainel0 
         BorderStyle     =   1  'Fixed Single
         DataField       =   "Alíquota do Anexo"
         Height          =   120
         Left            =   8190
         TabIndex        =   34
         Top             =   180
         Visible         =   0   'False
         Width           =   240
      End
      Begin VB.Label Label 
         Caption         =   "CEST........:"
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
         Index           =   9
         Left            =   3480
         TabIndex        =   35
         Top             =   1980
         Width           =   960
      End
      Begin VB.Label Label 
         Alignment       =   1  'Right Justify
         AutoSize        =   -1  'True
         Caption         =   "Unidade no Exterior:"
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
         Index           =   10
         Left            =   2625
         TabIndex        =   36
         Top             =   2355
         Width           =   1815
      End
      Begin VB.Label Label 
         AutoSize        =   -1  'True
         Caption         =   "ClassTrib:"
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
         Index           =   11
         Left            =   90
         TabIndex        =   37
         Top             =   2700
         Width           =   900
      End
      Begin VB.Label labFdo7 
         BackColor       =   &H00E0E0E0&
         BorderStyle     =   1  'Fixed Single
         BeginProperty Font 
            Name            =   "Microsoft Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H80000008&
         Height          =   300
         Left            =   2610
         TabIndex        =   38
         Top             =   2700
         Width           =   7140
      End
      Begin VB.Label Label 
         AutoSize        =   -1  'True
         Caption         =   "Red.IBS:"
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
         Index           =   12
         Left            =   90
         TabIndex        =   39
         Top             =   3050
         Width           =   840
      End
      Begin VB.Label Label 
         AutoSize        =   -1  'True
         Caption         =   "Red.CBS:"
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
         Index           =   13
         Left            =   2280
         TabIndex        =   40
         Top             =   3050
         Width           =   855
      End
      Begin VB.Label Label 
         AutoSize        =   -1  'True
         Caption         =   "Tipo Alíq:"
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
         Index           =   14
         Left            =   4380
         TabIndex        =   41
         Top             =   3050
         Width           =   930
      End
   End
   Begin IRRIG.GListV grdBrowse 
      Height          =   795
      Left            =   60
      TabIndex        =   42
      Top             =   330
      Width           =   1575
      _extentx        =   2778
      _extenty        =   1402
      fullrowselect   =   0
      rowheight       =   225
      allowedit       =   -1
      allowinsert     =   -1
      allowdelete     =   -1
      manualupdate    =   -1
      manualdelete    =   -1
      navigationaddmode=   1
      showfilterbar   =   -1
      showgridcaption =   0
      caption         =   ""
      font            =   "CLASSIFI.frx":0000
      cachesize       =   100
   End
End
Attribute VB_Name = "frmClassifi"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'* Sistema...: SISTEMA IRRIGAÇÃO PENÁPOLIS
'* Empresa...: IRRIGAÇÃO PENÁPOLIS
'* Módulo....: Classifi
'* Função....: Classificação Fiscal (NCM)
'* CopyRight.: (C)2024 IRRIGAÇÃO PENÁPOLIS
'* Criação...: GAS-2007 - Gerador Automático de Sistemas
'* Data......: 16/08/2024 14:57:57
'* * * * * * *

Option Explicit
DefInt A-Z

Public vgSituacao As Integer                      'situação de edição que do módulo
Public vgCaracteristica As Integer                'caracteristica do módulo
Public vgTipo As Integer                          'tipo do módulo
Public vgFiltroInicial As String                  'filtro inicial do módulo
Public vgOrdemInicial As String                   'ordem inicial do módulo
Public vgUltimaOrdem As String                    'última ordenação feita no módulo
Public vgUltimoFiltro As String                   'último filtro definido no módulo
Public vgUltimoFiltroComTit As String             'titulo do último filtro definido no módulo
Public vgUltimaOrdemComTit As String              'titulo da última ordenação feita no módulo
Public vgUltimoTabIndex As Integer                'último campo com foco do módulo
Public vgPriVez As Integer                        'flag de carregamento do módulo
Public WithEvents vgTb As GRecordSet              'tabela de dados do módulo
Attribute vgTb.VB_VarHelpID = -1
Public vgSQL As String                            'expressão SQL que define o módulo
Public vgTemInclusao As Integer                   'flag se tem ou não inclusão no módulo
Public vgTemExclusao As Integer                   'flag se tem ou não exclusão no módulo
Public vgTemProcura As Integer                    'flag se tem ou não procura no módulo
Public vgTemFiltro As Integer                     'flag se tem ou não filtro no módulo
Public vgTemAlteracao As Integer                  'flag se tem ou não alteração no módulo
Public vgTemAlteracaoGrids As Integer              'flag se tem ou não alteração nos grids
Public vgTemBrowse As Integer                     'flag se tem ou não janela em grade no módulo
Public vgSemVincDados As Integer                  'Flag para definir formulários sem vinculo com dados
Public vgTemCondicoesEsp As Integer               'flag se tem expresão de validação para inclusão/alteração ou exclusão
Public vgEmBrowse As Integer                      'flag se o módulo esta em grade
Public vgRepeticao As Integer                     'flag de repetição do último reg digitado
Public vgAlterar As Integer                       'flag de Alteracao de registros
Public vgUltAlterar As Integer                    'flag de última situação de "pode alterar"
Public vgFiltroEmUso As Integer                   'Indice do Filtro atual em uso
Public vgIndDefault As String                     'indice default do módulo
Public vgFormID As Long                           'identificador único para o módulo
Public vgIdentTab As String                       'nome da tabela principal do módulo
Public vgFrmImpCons As New frmImpCons             'impressao de consutlas
Public vgTooltips As New cTooltips                'classe de ajuda para os controes do módulo
Public vgFiltroOriginal As String
Dim txtCampo(7) As New FormataCampos              'classe dos campos tipo texto do módulo
Dim chkCampo(3) As New FormataCampos              'classe dos campos tipo lógico do módulo
Dim opcPainel0(2) As New FormataCampos
Dim opcPainel1(1) As New FormataCampos
Dim Sequencia_da_Classificacao As Integer, Porcentagem_do_IPI As Double, Anexo_da_Reducao As Integer
Dim Aliquota_do_Anexo As Integer, Produto_Diferido As Boolean, Reducao_de_Base_de_Calculo As Boolean
Dim Inativo As Boolean, Ncm As Long, Descricao_do_NCM As String
Dim IVA As Double, Tem_Convenio As Boolean, Cest As String
Dim Un_Exterior As String, Ajuste As String
Dim ClassTribId As Long                           'ID da ClassTrib vinculada
Dim vgGravarClassTribId As Boolean                'Flag para indicar que ClassTribId deve ser gravado via UPDATE
Public lblAjuste As Object

Public Anexo As Byte, Aliquota As Byte

'ROTINA MANUAL
'Proposito: arrumar o erro que tinha quando alterava um registro carregado por outro formulario
'evento - quando uma opção for selecionada
Private Sub opcPainel1Cp_Click(Index As Integer)
   If vgPriVez Then Exit Sub
   If opcPainel1(Index).Locked Then
      opcPainel1(val(labopcPainel1.Caption)).Value = True
   Else
      'If Val(labopcPainel1.Caption) <> opcPainel1(Index).BookMark Then 'manual
         labopcPainel1.Caption = Str$(opcPainel1(Index).BookMark)
         LigaFocos Me
         InicializaApelidos COM_TEXTBOX
         ExecutaVisivel
         ExecutaPreValidacao
         'Ação do Change (Adicionar pelo gas)
         opcPainel1(Index).Change
         Select Case Index
            Case 0
               Anexo = 0
            Case 1
               Anexo = 1
      End Select
      'End If 'Manual
   End If
End Sub

'ROTINA MANUAL
'Proposito: arrumar o erro que tinha quando alterava um registro carregado por outro formulario
'evento - quando uma opção for selecionada
'Private Sub opcPainel2Cp_Click(Index As Integer)
'   If vgPriVez Then Exit Sub
'   If opcPainel2(Index).Locked Then
'      opcPainel2(Val(labopcPainel2.Caption)).Value = True
'   Else
      'If Val(labopcPainel2.Caption) <> opcPainel2(Index).BookMark Then 'Manual
'         labopcPainel2.Caption = Str$(opcPainel2(Index).BookMark)
'         LigaFocos Me
'         InicializaApelidos COM_TEXTBOX
'         ExecutaVisivel
'         ExecutaPreValidacao
         'Ação do Change (Adicionar Pelo Gas)
'         opcPainel2(Index).Change
'         Select Case Index
'            Case 0
'               Aliquota = 0
'            Case 1
'               Aliquota = 1
'      End Select
'      'End If 'Manual
'   End If
'End Sub

Public Property Get txtCampoTab(Index As Integer) As FormataCampos
   Set txtCampoTab = txtCampo(Index)
End Sub

'inicializa variaveis (apelidos) coms os campos correspondentes
Private Sub InicializaApelidos(vgComOQue As Integer)
   Debug.Print ">>> InicializaApelidos INICIO - vgComOQue=" & vgComOQue
   On Error Resume Next                           'prepara para possíveis erros
   If (vgTb.RecordCount > 0 And vgTb.EOF = False And vgTb.BOF = False) Or vgComOQue = COM_TEXTBOX Then
      If vgComOQue = COM_TEXTBOX Then
         Debug.Print ">>> InicializaApelidos - COM_TEXTBOX - lendo campos 0-6"
         Sequencia_da_Classificacao = txtCampo(0).Value
         Porcentagem_do_IPI = txtCampo(3).Value
         Anexo_da_Reducao = val(labopcPainel1.Caption)
         Aliquota_do_Anexo = val(labopcPainel0.Caption)
         Produto_Diferido = chkCampo(1).Value
         Reducao_de_Base_de_Calculo = chkCampo(2).Value
         Inativo = chkCampo(0).Value
         Ncm = txtCampo(1).Value
         Descricao_do_NCM = txtCampo(2).Value
         IVA = txtCampo(4).Value
         Tem_Convenio = chkCampo(3).Value
         Cest = txtCampo(5).Value
         Un_Exterior = txtCampo(6).Value
         ' NOTA: ClassTribId NÃO é lido do campo de texto pois mostra o CodigoClassTrib
         ' O ClassTribId (Id real) é mantido pela variável e alterado apenas em:
         ' - COM_REGISTRO (carrega do banco)
         ' - bottxtCampo7_Click (pesquisa)
         Debug.Print ">>> InicializaApelidos - COM_TEXTBOX - ClassTribId mantido=" & ClassTribId
      Else
         Sequencia_da_Classificacao = IIf(IsNull(vgTb![Seqüência da Classificação]), 0, vgTb![Seqüência da Classificação])
         Porcentagem_do_IPI = IIf(IsNull(vgTb![Porcentagem do IPI]), 0, vgTb![Porcentagem do IPI])
         Anexo_da_Reducao = IIf(IsNull(vgTb![Anexo da Redução]), 0, vgTb![Anexo da Redução])
         Aliquota_do_Anexo = IIf(IsNull(vgTb![Alíquota do Anexo]), 0, vgTb![Alíquota do Anexo])
         Produto_Diferido = IIf(IsNull(vgTb![Produto Diferido]), 0, vgTb![Produto Diferido])
         Reducao_de_Base_de_Calculo = IIf(IsNull(vgTb![Redução de Base de Cálculo]), 0, vgTb![Redução de Base de Cálculo])
         Inativo = IIf(IsNull(vgTb!Inativo), 0, vgTb!Inativo)
         Ncm = IIf(IsNull(vgTb!Ncm), 0, vgTb!Ncm)
         Descricao_do_NCM = IIf(IsNull(vgTb![Descrição do NCM]), "", vgTb![Descrição do NCM])
         IVA = IIf(IsNull(vgTb!IVA), 0, vgTb!IVA)
         Tem_Convenio = IIf(IsNull(vgTb![Tem Convênio]), 0, vgTb![Tem Convênio])
         Cest = IIf(IsNull(vgTb!Cest), "", vgTb!Cest)
         Un_Exterior = IIf(IsNull(vgTb![Un Exterior]), "", vgTb![Un Exterior])
         ' Lê ClassTribId do registro atual usando SELECT direto (campo pode não estar no GRecordSet)
         Debug.Print ">>> InicializaApelidos - COM_REGISTRO - Seq=" & Sequencia_da_Classificacao & " NCM=" & Ncm
         ClassTribId = 0
         If Sequencia_da_Classificacao > 0 Then
            Dim TbClassTribId As GRecordSet
            On Error Resume Next
            Set TbClassTribId = vgDb.OpenRecordSet("SELECT ClassTribId FROM [Classificação Fiscal] WHERE [Seqüência da Classificação] = " & Sequencia_da_Classificacao)
            If Err Then
               Debug.Print ">>> InicializaApelidos - COM_REGISTRO - ERRO ao abrir SELECT: " & Err.Description
               Err.Clear
            ElseIf Not TbClassTribId Is Nothing Then
               If TbClassTribId.RecordCount > 0 Then
                  If Not IsNull(TbClassTribId!ClassTribId) Then
                     ClassTribId = TbClassTribId!ClassTribId
                  End If
               End If
               TbClassTribId.CloseRecordset
               Set TbClassTribId = Nothing
            End If
            On Error GoTo 0
         End If
         Debug.Print ">>> InicializaApelidos - COM_REGISTRO - ClassTribId final=" & ClassTribId
         ' Atualiza os campos do ClassTrib ao carregar registro
         AtualizaClassTribCompleto
      End If
   End If
   Debug.Print ">>> InicializaApelidos FIM"
   If Err Then Err.Clear                          'se deu algum erro, vamos resetá-lo
End Sub

'verifica permissões para as condições especiais
'e muda situação de alguns botões
Private Sub AnalisaCondicoes()
   Dim i As Integer
   On Error Resume Next
   If Not mdiIRRIG.ActiveForm Is Nothing Then
      If mdiIRRIG.ActiveForm.Name <> Me.Name And Me.Visible Then Exit Sub
   End If
   With mdiIRRIG
      i = Permitido(vgIdentTab, ACAO_INCLUINDO)
      If Err Then Err.Clear: i = False
      vgTemInclusao = i
      grdBrowse.AllowInsert = i
      .botInclui.Enabled = i
      .Menu_Inclui.Enabled = i
      i = Permitido(vgIdentTab, ACAO_EXCLUINDO)
      If Err Then Err.Clear: i = False
      vgTemExclusao = i
      grdBrowse.AllowDelete = i
      .botExclui.Enabled = i
      .Menu_Exclui.Enabled = i
      i = Permitido(vgIdentTab, ACAO_EDITANDO)
      If Err Then Err.Clear: i = False
      vgTemAlteracao = i
      grdBrowse.AllowEdit = i And vgAlterar
      .Menu_Paltera.Enabled = i
      LigaDesligaControles Me, Not i
   End With
End Sub

'executa processos/validacoes nos campos do arquivo
Public Function Executar(vgOq As String, Optional ByRef vgColumn As Integer) As String
   Dim i As Integer, vgRsError As GRecordSet, vgMsg As String, vgOk As Integer, vgPV As Boolean, vgNVez As Integer, vgInd As Integer
   On Error GoTo DeuErro                          'fica na espera de um erro...
   vgMsg$ = ""                                    'retorna uma msg dizendo o motivo
   vgOk = True                                    'se a validação esta OK
   vgPV = vgPriVez
   vgColumn = -1
   vgNVez = 0                                     'porque não fez o processo/validacoes
   If vgOq = VALIDACOES Then
      InicializaApelidos COM_TEXTBOX
      vgOk = (Ncm > 0)
      vgMsg$ = "NCM inválido!"
      If Not vgOk Then vgColumn = 2
      If vgOk Then
         vgOk = (Not Vazio(Descricao_do_NCM))
         vgMsg$ = "Descrição do NCM não pode ser vazio!"
         If Not vgOk Then vgColumn = 3
      End If
      If vgOk Then
         vgMsg$ = ""
      ElseIf vgColumn <> -1 And Not vgEmBrowse Then
         txtCampo(vgColumn - 1).SetFocus
      End If
      DoEvents
   ElseIf vgOq = INICIALIZACOES Then
      If vgPriVez = False Then
         vgPriVez = True
         For i = 0 To UBound(txtCampo)
            If Len(txtCampo(i).DataField) > 0 Then
               txtCampo(i).Text = ""
            End If
         Next
         InicializaApelidos COM_TEXTBOX
         On Error Resume Next
         chkCampo(0).Value = False
         chkCampo(1).Value = False
         chkCampo(2).Value = False
         opcPainel1(0).Value = True
         opcPainel0(0).Value = True
         chkCampo(3).Value = False
         labFdo7.Caption = ""
         labFdo7.ToolTipText = ""
         On Error GoTo DeuErro
         InicializaApelidos COM_TEXTBOX
      End If
   ElseIf vgOq = PEGA_DO_ARQUIVO Then
      If vgTb.RecordCount > 0 And vgTb.EOF = False And vgTb.BOF = False Then
         vgPriVez = True
         vgTb.Resync 1             'adAffectCurrent
         InicializaApelidos COM_REGISTRO
         For i = 0 To UBound(txtCampo)
            If Len(txtCampo(i).DataField) > 0 Then
               txtCampo(i).SetOriginalValue = True
               txtCampo(i).Value = vgTb.Fields(txtCampo(i).DataField).Value
            End If
         Next
         chkCampo(0).Value = Inativo
         chkCampo(1).Value = Produto_Diferido
         chkCampo(2).Value = Reducao_de_Base_de_Calculo
         opcPainel1(Anexo_da_Reducao).Value = True
         opcPainel0(Aliquota_do_Anexo).Value = True
         chkCampo(3).Value = Tem_Convenio
         ' Carregar descrição do ClassTrib vinculado
         AtualizaDescricaoClassTrib
         If vgSituacao = ACAO_NAVEGANDO Then
            If Me.Name = mdiIRRIG.ActiveForm.Name Then
               If Not ActiveControl Is Nothing Then
                  If TypeOf ActiveControl Is GListV Then
                     If Not ActiveControl.PreEditing Then DoEvents
                  Else
                     DoEvents
                  End If
               End If
            End If
         End If
      Else
         Executar INICIALIZACOES
      End If
      vgPriVez = False
   ElseIf vgOq = TESTA_VAL_RS Then
      vgTb.Resync 1         'adAffectCurrent
      For i = 0 To UBound(txtCampo)
         If Len(txtCampo(i).DataField) > 0 Then
            If vgTb.Fields(txtCampo(i).DataField).Value <> txtCampo(i).OriginalValue Then
               If Len(vgMsg$) = 0 Then
                  vgMsg$ = Caption + "|" + CStr(3600 + Abs(vgEmBrowse)) + "|" + LoadGasString(122)
               End If
               If vgEmBrowse Then
                  Exit For
               Else
                  vgPriVez = True
                  txtCampo(i).SetOriginalValue = True
                  txtCampo(i).Value = vgTb.Fields(txtCampo(i).DataField).Value
                  vgPriVez = False
               End If
            End If
         End If
      Next
   ElseIf vgOq = POE_NO_ARQUIVO Then
      vgGravarClassTribId = False  ' Reset do flag antes de cada gravação
      For i = 0 To UBound(txtCampo)
         If Len(txtCampo(i).DataField) > 0 Then
            If Not vgTb.Table.Columns(txtCampo(i).DataField).SeqInterno Then
               If (txtCampo(i).Value & "" <> vgTb.Fields(txtCampo(i).DataField).Value & "") Or _
                        (IsNull(txtCampo(i).Value) Xor IsNull(vgTb.Fields(txtCampo(i).DataField).Value)) Then    'se for diferente do conteúdo atual do RS
                  vgTb.Fields(txtCampo(i).DataField).Value = txtCampo(i).Value
               End If
            End If
         End If
      Next
      InicializaApelidos COM_TEXTBOX
      vgTb![Anexo da Redução] = Anexo_da_Reducao
      vgTb![Alíquota do Anexo] = Aliquota_do_Anexo
      vgTb![Produto Diferido] = Produto_Diferido
      vgTb![Redução de Base de Cálculo] = Reducao_de_Base_de_Calculo
      vgTb!Inativo = Inativo
      vgTb![Tem Convênio] = Tem_Convenio
      ' Grava ClassTribId usando UPDATE direto (campo pode não estar no GRecordSet)
      ' O UPDATE será executado APÓS o registro ser salvo, então usamos um flag
      vgGravarClassTribId = True
   ElseIf vgOq = INI_APELIDOS Then
      InicializaApelidos COM_REGISTRO
      ' Se é inclusão, limpa ClassTribId e campos visuais
      If vgSituacao = ACAO_INCLUINDO Then
         ClassTribId = 0
         LimpaClassTribCampos
         txtCp(7).Text = ""
      End If
      ExecutaVisivel
      ExecutaPreValidacao
   ElseIf vgOq = PODE_ALTERAR Then
      vgOk = (vgSituacao = ACAO_INCLUINDO Or vgAlterar)
      For i = 0 To UBound(txtCampo)
         If Len(txtCampo(i).DataField) > 0 Then
            txtCampo(i).Locked = Not (vgOk And txtCampo(i).Editable)
         End If
      Next
      For i = 0 To UBound(chkCampo)
         If Len(chkCampo(i).DataField) > 0 Then
            chkCampo(i).Locked = Not (vgOk And chkCampo(i).Editable)
         End If
      Next
      For i = 0 To UBound(opcPainel1)
         If Len(opcPainel1(i).DataField) > 0 Then
            If Not opcPainel1(i).Value Then    'vamos primeiro desabilitar os não selecionados
               opcPainel1(i).Locked = Not (vgOk And opcPainel1(i).Editable)
            Else
               vgInd = i
            End If
         End If
      Next
      opcPainel1(vgInd).Locked = False
      opcPainel1(vgInd).Value = True
      opcPainel1(vgInd).Locked = Not (vgOk And opcPainel1(vgInd).Editable)
      For i = 0 To UBound(opcPainel0)
         If Len(opcPainel0(i).DataField) > 0 Then
            If Not opcPainel0(i).Value Then    'vamos primeiro desabilitar os não selecionados
               opcPainel0(i).Locked = Not (vgOk And opcPainel0(i).Editable)
            Else
               vgInd = i
            End If
         End If
      Next
      opcPainel0(vgInd).Locked = False
      opcPainel0(vgInd).Value = True
      opcPainel0(vgInd).Locked = Not (vgOk And opcPainel0(vgInd).Editable)
      ExecutaPreValidacao
   ElseIf vgOq = APOS_EDICAO Then
      On Error GoTo DeuErro
      ' Grava ClassTribId via UPDATE direto ANTES de recarregar o registro
      If vgGravarClassTribId Then
         Debug.Print ">>> APOS_EDICAO - Gravando ClassTribId=" & ClassTribId & " para Seq=" & Sequencia_da_Classificacao
         On Error Resume Next
         If ClassTribId > 0 Then
            vgDb.Execute "UPDATE [Classificação Fiscal] SET ClassTribId = " & ClassTribId & " WHERE [Seqüência da Classificação] = " & Sequencia_da_Classificacao
         Else
            vgDb.Execute "UPDATE [Classificação Fiscal] SET ClassTribId = NULL WHERE [Seqüência da Classificação] = " & Sequencia_da_Classificacao
         End If
         If Err Then
            Debug.Print ">>> APOS_EDICAO - ERRO no UPDATE: " & Err.Description
            Err.Clear
         Else
            Debug.Print ">>> APOS_EDICAO - UPDATE executado com sucesso"
         End If
         On Error GoTo DeuErro
         vgGravarClassTribId = False
      End If
      ' Agora recarrega o registro (e vai ler o ClassTribId correto do banco)
      InicializaApelidos COM_REGISTRO
      If Abs(vgSituacao) = ACAO_EDITANDO Then
         vgDb.Execute "Update [Classificação Fiscal] Set [Anexo da Redução] = " & Anexo & ", [Alíquota do Anexo] = " & Aliquota & " Where [Seqüência da Classificação] = " & Sequencia_da_Classificacao
      End If
   End If
   Executar = vgMsg$                              'prepara saida da função
   vgPriVez = vgPV
   Exit Function                                  'e cai fora...

DeuErro:
   Select Case Err                                'vamos verificar se deu algum erro

      Case -2147467259
         Resume Next

      Case -2147217885                            'registro foi apagado
         vgPriVez = False
         MoveRegistro Me, REG_FORCAVOLTA          'volta um registro
         PrepBotoes Me, vgSituacao                'acerta icones dos botoes

   End Select
   Executar = Err.Source + "|" + Trim$(Str$(Err)) + "|" + Error$ 'não teve jeito o erro não pode ser evitado...
   If Err = 3265 Then Executar = Executar & vbCrLf & vbCrLf & txtCampo(i).DataField
   If Not vgRsError Is Nothing Then
      vgRsError.CancelUpdate
      Set vgRsError = Nothing
   End If
   vgPriVez = vgPV
End Function

Private Sub grdBrowse_DeleteData(ByVal vgItem As Long, vgColumns() As Variant, vgDataDeleted As Boolean, vgErrorMessage As String)
   vgDataDeleted = mdiIRRIG.ExcluiRegistro()
End Sub
   
Private Sub grdBrowse_InitEdit(CancelEdit As Boolean)
   Reposition
End Sub

Private Sub grdBrowse_ItemSelect(ByVal vgItem As Long, vgColumns() As Variant)
   If vgPriVez Or Not grdBrowse.Visible Then Exit Sub
   If vgSituacao = ACAO_NAVEGANDO Then Executar PEGA_DO_ARQUIVO
End Sub

'evento disparado ao mudar de registro no grid.
Private Sub grdBrowse_SkipRecord(Columns() As Variant, ByVal BookMark As Variant)
   If vgSituacao = ACAO_NAVEGANDO Then Reposition
End Sub

Private Sub grdBrowse_GetColumnFilter(ByVal vgColumn As Integer, vgColumns() As Variant, vgFilter As String)
   If UBound(txtCampo) >= vgColumn - 1 Then
      vgFilter = txtCampo(vgColumn - 1).Filter
   End If
End Sub

   
'executa a pré-validação da coluna do grid do modo grade do formulário
Private Sub grdBrowse_GetColumnLocked(ByVal vgRow As Long, ByVal vgCol As Long, vgColumns() As Variant, ByRef FormField As FormataCampos, ByRef vgLocked As Boolean)
   ExecutaPreValidacao                            'checa as pré-validações
   vgLocked = Not FormField.Enabled               'aplica as definições de pré-validação que são aplicadas ao campo da tela
End Sub


Private Sub grdBrowse_SaveData(ByVal vgItem As Long, vgColumns() As Variant, vgDataSaved As Boolean, vgColumn As Integer, vgErrorMessage As String)
   mdiIRRIG.SalvaDados vgColumn
   vgDataSaved = (vgSituacao = ACAO_NAVEGANDO)
End Sub
   
Private Sub grdBrowse_StatusChanged(ByVal vgNewStatus As Integer)
   If (vgNewStatus = ACAO_EXCLUINDO And val(grdBrowse.RecordSet.BookMark) >= 0) Then
      Reposition
   End If
   PrepBotoes Me, vgNewStatus                          'acerta icones dos botoes
   mdiIRRIG.RemontaForm                                'remonta dos os form da tela
End Sub

'apresenta popup menu para trabalhar com o grid
Private Sub grdBrowse_MouseUp(Button As Integer, Shift As Integer, x As Single, y As Single, ByVal vgCurCol As Integer)
   MostraPopGrid Me, Button
End Sub

'liga/desliga flag de repetição do último reg visualizado
Public Sub LigaDesligaAlterar()
   vgAlterar = Not vgAlterar
   vgUltAlterar = vgAlterar                            'guarda situação de "pode alterar"
   AnalisaCondicoes                                    'vamos atualizar as condições para inclusão, exclusão, alteração...
   ExecutaVisivel
   PrepBotoes Me, vgSituacao                           'acerta icones dos botoes
End Sub

'evento - quando qq tecla for digitada no formulário
Private Sub Form_KeyPress(KeyAscii As Integer)
   Dim Ok As Boolean
   If Not Me.ActiveControl Is Nothing Then
      Ok = (Not TypeOf Me.ActiveControl Is GListV)         'se não está em um GRID
   Else
      Ok = True
   End If
   If Not Ok Then
      Ok = (Me.ActiveControl.Status = ACAO_NAVEGANDO And Not Me.ActiveControl.PreEditing) 'e se grid não está em pré-edição, edição nem inclusão
   End If
   If KeyAscii = vbKeyEscape And Ok Then                                                  'se teclou ESC
      Unload Me                                   'tira este form da memória
   End If
End Sub

'evento - quando o formuário for pintado
Private Sub Form_Paint()
   grdBrowse.Visible = vgEmBrowse                 'AH VB!!...
End Sub

'prepara botões e o formulário para o novo registro
Public Sub Reposition(Optional ForceRebind As Boolean, Optional LockGrids As Boolean = True)
   On Error GoTo DeuErro
   If vgPriVez Then Exit Sub
   If vgSituacao <> ACAO_INCLUINDO And vgSituacao <> ACAO_EDITANDO Then Executar PEGA_DO_ARQUIVO
   Anexo = Anexo_da_Reducao: Aliquota = Aliquota_do_Anexo
   ExecutaVisivel
   ExecutaPreValidacao
   vgTemAlteracaoGrids = Not LockGrids
   Executar PODE_ALTERAR
   If vgEmBrowse And vgSituacao = ACAO_NAVEGANDO And vgFrmImpCons Is Nothing Then grdBrowse.Refresh
DeuErro:
   
End Sub

'executa a pré-validação dos campos
Private Sub ExecutaPreValidacao()
   Dim Ok As Boolean, vgPV As Integer
   On Error Resume Next                           'prepara para possiveis erros
   vgPV = vgPriVez
   vgPriVez = True
   Ok = (Inativo = False)
   Label(1).Enabled = Ok And vgAlterar
   Ok = (Reducao_de_Base_de_Calculo)
   Label(7).Enabled = Ok And vgAlterar
   Ok = (Reducao_de_Base_de_Calculo)
   Label(8).Enabled = Ok And vgAlterar
   Ok = (Reducao_de_Base_de_Calculo)
   Painel(1).Enabled = Ok And vgAlterar
   Ok = (Reducao_de_Base_de_Calculo)
   opcPainel1(0).Locked = Not (vgAlterar And opcPainel1(0).Editable)
   opcPainel1(0).Enabled = Ok Or Not vgAlterar
   Ok = (Reducao_de_Base_de_Calculo)
   opcPainel1(1).Locked = Not (vgAlterar And opcPainel1(1).Editable)
   opcPainel1(1).Enabled = Ok Or Not vgAlterar
   Ok = (Reducao_de_Base_de_Calculo = True)
   chkCampo(3).Locked = Not (vgAlterar And chkCampo(3).Editable)
   chkCampo(3).Enabled = Ok Or Not vgAlterar
   If Err Then Err.Clear                          'se houve erro, limpa...
   vgPriVez = vgPV
End Sub

'coloca os campos visíveis segundo a condição
Private Sub ExecutaVisivel()
   On Error Resume Next                           'prepara para possiveis erros
   Label(2).Visible = (False)
   If Err Then Err.Clear                          'se houve erro, limpa...
End Sub

'evento - quando o conteúdo do campo for alterado
Private Sub txtCp_Change(Index As Integer)
   Debug.Print ">>> txtCp_Change INICIO - Index=" & Index
   If vgPriVez Or txtCampo(Index).PriVez Then
      Debug.Print ">>> txtCp_Change - Saindo (PriVez)"
      Exit Sub
   End If
   Debug.Print ">>> txtCp_Change - Antes LigaFocos"
   If Len(txtCampo(Index).DataField) > 0 Then LigaFocos Me
   Debug.Print ">>> txtCp_Change - Antes InicializaApelidos"
   InicializaApelidos COM_TEXTBOX                         'inicializa apelidos com o que esta sendo digitado
   Debug.Print ">>> txtCp_Change - Antes txtCampo.Change"
   txtCampo(Index).Change
   
   ' Para o campo 7 (ClassTrib): Atualiza automaticamente quando o código mudar
   If Index = 7 Then
      Dim strCodigoAtual As String
      Dim TbBuscaIdChange As GRecordSet
      
      strCodigoAtual = Trim$(txtCp(7).Text)
      
      If Len(strCodigoAtual) = 0 Then
         ' Campo limpo - zera ClassTribId
         Debug.Print ">>> txtCp_Change - Campo 7 limpo, zerando ClassTribId"
         If ClassTribId <> 0 Then
            ClassTribId = 0
            LimpaClassTribCampos
         End If
      Else
         ' Há um código - busca o Id e atualiza os campos
         Debug.Print ">>> txtCp_Change - Campo 7 alterado, buscando ClassTribId para codigo=[" & strCodigoAtual & "]"
         On Error Resume Next
         Set TbBuscaIdChange = vgDb.OpenRecordSet( _
            "SELECT Id FROM ClassTrib WHERE CodigoClassTrib = '" & strCodigoAtual & "'")
         If Err Then
            Debug.Print ">>> txtCp_Change - ERRO ao buscar Id: " & Err.Description
            Err.Clear
         ElseIf Not TbBuscaIdChange Is Nothing Then
            If TbBuscaIdChange.RecordCount > 0 And Not TbBuscaIdChange.EOF Then
               Dim NovoClassTribId As Long
               NovoClassTribId = TbBuscaIdChange!Id
               If NovoClassTribId <> ClassTribId Then
                  ClassTribId = NovoClassTribId
                  Debug.Print ">>> txtCp_Change - ClassTribId atualizado=" & ClassTribId
                  AtualizaClassTribCompleto
                  ' Marca formulário como alterado para habilitar botão Gravar
                  LigaFocos Me
               End If
            Else
               Debug.Print ">>> txtCp_Change - Codigo nao encontrado na tabela ClassTrib"
            End If
            TbBuscaIdChange.CloseRecordset
            Set TbBuscaIdChange = Nothing
         End If
         On Error GoTo 0
      End If
   End If
   
   Debug.Print ">>> txtCp_Change FIM"
End Sub

'evento - quando o campo receber o foco
Private Sub txtCp_GotFocus(Index As Integer)
   txtCampo(Index).GotFocus
End Sub

'evento - quando qq tecla for digitada no campo
Private Sub txtCp_KeyDown(Index As Integer, KeyCode As Integer, Shift As Integer)
   Debug.Print ">>> txtCp_KeyDown INICIO - Index=" & Index & " KeyCode=" & KeyCode
   On Error GoTo ErroKeyDown
   If KeyCode = vbKeyReturn And vgSituacao <> ACAO_NAVEGANDO Then  'se tela esta em edição e digitou ENTER
      Debug.Print ">>> txtCp_KeyDown - Antes ExecutaVisivel"
      ExecutaVisivel                                               'torna camos visiveis
      Debug.Print ">>> txtCp_KeyDown - Antes ExecutaPreValidacao"
      ExecutaPreValidacao                                          'habilita/desabilita campos
      Debug.Print ">>> txtCp_KeyDown - Depois ExecutaPreValidacao"
   End If
   Debug.Print ">>> txtCp_KeyDown - Antes txtCampo.KeyDown - txtCampo(" & Index & ") Is Nothing = " & (txtCampo(Index) Is Nothing)
   Debug.Print ">>> txtCp_KeyDown - txtCampo(" & Index & ").CtPri Is Nothing = " & (txtCampo(Index).CtPri Is Nothing)
   Debug.Print ">>> txtCp_KeyDown - txtCampo(" & Index & ").ListFields = " & txtCampo(Index).ListFields
   Debug.Print ">>> txtCp_KeyDown - txtCampo(" & Index & ").Source = " & txtCampo(Index).Source
   txtCampo(Index).KeyDown KeyCode, Shift
   Debug.Print ">>> txtCp_KeyDown FIM"
   Exit Sub
ErroKeyDown:
   Debug.Print ">>> txtCp_KeyDown ERRO: " & Err.Number & " - " & Err.Description
   Debug.Print ">>> txtCp_KeyDown - Linha com erro"
End Sub

'evento - quando qq tecla for digitada no campo
Private Sub txtCp_KeyPress(Index As Integer, KeyAscii As Integer)
   Debug.Print ">>> txtCp_KeyPress - Index=" & Index & " KeyAscii=" & KeyAscii
   txtCampo(Index).KeyPress KeyAscii
End Sub

'evento - quando o campo perder o foco
Private Sub txtCp_LostFocus(Index As Integer)
   Dim TbBuscaId As GRecordSet
   Debug.Print ">>> txtCp_LostFocus INICIO - Index=" & Index
   Debug.Print ">>> txtCp_LostFocus - Antes txtCampo.LostFocus"
   txtCampo(Index).LostFocus
   Debug.Print ">>> txtCp_LostFocus - Depois txtCampo.LostFocus"
   If vgSituacao <> ACAO_NAVEGANDO Then           'se tela esta em edição
      Debug.Print ">>> txtCp_LostFocus - Antes InicializaApelidos"
      InicializaApelidos COM_TEXTBOX              'pega apelidos dos campos
      Debug.Print ">>> txtCp_LostFocus - Antes ExecutaVisivel"
      ExecutaVisivel                              'torna camos visiveis
      Debug.Print ">>> txtCp_LostFocus - Antes ExecutaPreValidacao"
      ExecutaPreValidacao                         'habilita/desabilita campos
      Debug.Print ">>> txtCp_LostFocus - Depois ExecutaPreValidacao"
      
      ' Para campo 7 (ClassTrib): valida código digitado manualmente
      If Index = 7 Then
         Debug.Print ">>> txtCp_LostFocus - Validando ClassTrib - Texto=[" & txtCp(7).Text & "]"
         If Len(Trim$(txtCp(7).Text)) > 0 Then
            On Error Resume Next
            ' Busca o Id pelo código digitado
            Set TbBuscaId = vgDb.OpenRecordSet( _
               "SELECT Id, CodigoClassTrib, DescricaoClassTrib FROM ClassTrib WHERE CodigoClassTrib = '" & Trim$(txtCp(7).Text) & "'")
            If Not TbBuscaId Is Nothing Then
               If Not TbBuscaId.EOF Then
                  ' Código válido encontrado
                  If ClassTribId <> TbBuscaId!Id Then
                     ClassTribId = TbBuscaId!Id
                     Debug.Print ">>> txtCp_LostFocus - ClassTribId encontrado=" & ClassTribId
                     AtualizaClassTribCompleto
                     LigaFocos Me
                  End If
               Else
                  ' Código não encontrado - limpa
                  Debug.Print ">>> txtCp_LostFocus - ClassTrib não encontrado, limpando"
                  ClassTribId = 0
                  LimpaClassTribCampos
                  txtCp(7).Text = ""
               End If
               TbBuscaId.CloseRecordset
               Set TbBuscaId = Nothing
            End If
            On Error GoTo 0
         End If
      End If
   End If
   Debug.Print ">>> txtCp_LostFocus FIM"
End Sub


'evento - quando o check for marcado/desmarcado
Private Sub chkCp_Click(Index As Integer)
   If vgPriVez Then Exit Sub
   If chkCampo(Index).Locked Then
      If Index = 0 Then
         chkCampo(0).Value = Inativo
      ElseIf Index = 1 Then
         chkCampo(1).Value = Produto_Diferido

      ElseIf Index = 2 Then
         chkCampo(2).Value = Reducao_de_Base_de_Calculo

      ElseIf Index = 3 Then
         chkCampo(3).Value = Tem_Convenio
      End If
   Else
   If Len(chkCampo(Index).DataField) > 0 Then LigaFocos Me
      InicializaApelidos COM_TEXTBOX
      ExecutaVisivel                              'torna camos visiveis
      ExecutaPreValidacao                         'habilita/desabilita campos
      chkCampo(Index).Change
   End If
End Sub

'evento - quando o check receber o foco
Private Sub chkCp_GotFocus(Index As Integer)
   chkCampo(Index).GotFocus
End Sub

'evento - quando qq tecla for digitada no check
Private Sub chkCp_KeyDown(Index As Integer, KeyCode As Integer, Shift As Integer)
   chkCampo(Index).KeyDown KeyCode, Shift
End Sub

'evento - quando qq tecla for digitada no check
Private Sub chkCp_KeyPress(Index As Integer, KeyAscii As Integer)
   chkCampo(Index).KeyPress KeyAscii
End Sub

'evento - quando o check perder o foco
Private Sub chkCp_LostFocus(Index As Integer)
   chkCampo(Index).LostFocus
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
Private Sub opcPainel0Cp_Click(Index As Integer)
   If vgPriVez Then Exit Sub
   If opcPainel0(Index).Locked Then
      opcPainel0(val(labopcPainel0.Caption)).Value = True
   Else
      If val(labopcPainel0.Caption) <> opcPainel0(Index).BookMark Then
         labopcPainel0.Caption = Str$(opcPainel0(Index).BookMark)
         LigaFocos Me
         InicializaApelidos COM_TEXTBOX
         ExecutaVisivel
         ExecutaPreValidacao
         opcPainel0(Index).Change
         Select Case Index
            Case 0
               Aliquota = 0
            Case 1
               Aliquota = 1
            Case 2
               Aliquota = 2
      End Select
      End If
   End If
End Sub

'evento - quando qq tecla for digitada no campo
Private Sub opcPainel0Cp_KeyPress(Index As Integer, KeyAscii As Integer)
   opcPainel0(Index).KeyPress KeyAscii
End Sub

'evento - quando qq tecla for digitada no campo
Private Sub opcPainel0Cp_KeyDown(Index As Integer, KeyCode As Integer, Shift As Integer)
   opcPainel0(Index).KeyDown KeyCode, Shift
End Sub

'evento - quando o campo receber o foco
Private Sub opcPainel0Cp_GotFocus(Index As Integer)
   opcPainel0(Index).GotFocus
End Sub

'evento - quando o campo perder o foco
Private Sub opcPainel0Cp_LostFocus(Index As Integer)
   opcPainel0(Index).LostFocus
End Sub

'evento - quando o formulário receber o foco
Private Sub Form_Activate()
   PosicionaRegistro frmClassifi, "Seqüência da Classificação", seqRegistro: Anexo = Anexo_da_Reducao: Aliquota = Aliquota_do_Anexo
   AtivaForm Me
   
   'se tiver imprimindo registros em grade, fecha form de selecao/preview
   If FormEstaAberto("frmEnviaEmail") Then
      If Not frmEnviaEMail.Visible Then
         Unload vgFrmImpCons
         Set vgFrmImpCons = Nothing
         Unload frmEnviaEMail
      End If
   End If
End Sub

'evento - inicialização do formulário
Private Sub Form_Load()
   On Error GoTo DeuErro
   Screen.MousePointer = vbHourglass
   Caption = LoadGasString(15420)
   vgFormID = 630
   vgIdentTab = "Classificação Fiscal"
   vgFiltroEmUso = -1
   vgIndDefault = "Seqüência da Classificação"
   vgPriVez = True
   vgTipo = TP_TABELA
   vgTemInclusao = True
   vgTemExclusao = True
   vgTemAlteracao = True
   vgTemProcura = True
   vgTemFiltro = True
   vgTemBrowse = True
   grdBrowse.Tag = 1
   vgRepeticao = -99
   vgAlterar = False
   vgUltAlterar = False
   vgTemCondicoesEsp = False
   vgCaracteristica = F_DADOS
   vgUltimoTabIndex = 18
   vgSituacao = ACAO_NAVEGANDO
   Set picBox(0).Picture = LoadPicture(LoadGasPicture(6925))
   Set lblAjuste = Label(2)
   Set vgTb = New GRecordSet
   If Len(vgFiltroInicial$) > 0 Then
      vgFiltroInicial$ = vgFiltroInicial$ + " And "
   End If
   vgFiltroInicial$ = vgFiltroInicial$ + "[Seqüência da Classificação] > " & 0
   vgFiltroOriginal$ = vgFiltroInicial$
   DefineControles
   vgTooltips.Create
   Label(0).Caption = LoadGasString(15425)
   vgTooltips.AddTool txtCampo(1).CtPri, 0, LoadGasString(15426)
   Label(1).Caption = LoadGasString(15427)
   Label(2).Caption = LoadGasString(15428)
   vgTooltips.AddTool chkCampo(0).CtPri, 0, LoadGasString(15429)
   chkCampo(0).Caption = LoadGasString(15430)
   Label(3).Caption = LoadGasString(15431)
   Label(4).Caption = LoadGasString(15432)
   Label(5).Caption = LoadGasString(15433)
   vgTooltips.AddTool txtCampo(3).CtPri, 0, LoadGasString(15434)
   vgTooltips.AddTool chkCampo(1).CtPri, 0, LoadGasString(15435)
   chkCampo(1).Caption = LoadGasString(15436)
   vgTooltips.AddTool chkCampo(2).CtPri, 0, LoadGasString(15437)
   chkCampo(2).Caption = LoadGasString(15438)
   Label(6).Caption = LoadGasString(15439)
   vgTooltips.AddTool txtCampo(4).CtPri, 0, LoadGasString(15440)
   Label(7).Caption = LoadGasString(15441)
   Label(8).Caption = LoadGasString(15442)
   vgTooltips.AddTool opcPainel1(0).CtPri, 0, LoadGasString(15443)
   opcPainel1(0).Caption = LoadGasString(15444)
   vgTooltips.AddTool opcPainel1(1).CtPri, 0, LoadGasString(15445)
   opcPainel1(1).Caption = LoadGasString(15446)
   vgTooltips.AddTool opcPainel0(2).CtPri, 0, LoadGasString(15447)
   opcPainel0(2).Caption = LoadGasString(15448)
   vgTooltips.AddTool opcPainel0(0).CtPri, 0, LoadGasString(15449)
   opcPainel0(0).Caption = LoadGasString(15450)
   vgTooltips.AddTool chkCampo(3).CtPri, 0, LoadGasString(15451)
   chkCampo(3).Caption = LoadGasString(15452)
   Label(9).Caption = LoadGasString(15453)
   vgTooltips.AddTool txtCampo(5).CtPri, 0, LoadGasString(15454)
   vgTooltips.AddTool opcPainel0(1).CtPri, 0, LoadGasString(15455)
   opcPainel0(1).Caption = LoadGasString(15456)
   Label(10).Caption = LoadGasString(15457)
   vgTooltips.AddTool txtCampo(6).CtPri, 0, LoadGasString(15458)
   Label(11).Caption = "ClassTrib:"
   vgTooltips.AddTool txtCampo(7).CtPri, 0, "Classificação Tributária IBS/CBS (NFe 5.0)"
   AjustaTamanho Me
   ' Garante altura mínima para mostrar todos os campos ClassTrib
   If Me.Height < 5300 Then Me.Height = 5300
   IniciaFormDados Me
   vgPriVez = False
   Reposition
   Screen.MousePointer = vbDefault
   Exit Sub

DeuErro:
   CErr.NumErro = Err
   CErr.FunctionName = "IniciaForm"
   CErr.Origem = CStr(vgFormID) + " - " + Me.Caption
   CErr.Show
End Sub

Public Sub DefineControles()
 On Error GoTo DeuErro
 grdBrowse.AddControlIgnoreFocus mdiIRRIG.botCancela.hWnd           'não deixa o grid tentar gravar automaticamente
 grdBrowse.AddControlIgnoreFocus mdiIRRIG.botSalva.hWnd             'se estiver perdendo o foco para esses botões
   grdBrowse.AllowDelete = True
   grdBrowse.AllowEdit = vgAlterar
   grdBrowse.SpecialPopupDisabled POP_GRID_BARS

   Set txtCampo(0).CtPri = txtCp(0)
   txtCampo(0).DataType = 1
   txtCampo(0).Mask = "9999"
   txtCampo(0).Editable = False
   txtCampo(0).BoundColumn = ""
   txtCampo(0).ListFields = ""
   txtCampo(0).OrderFields = ""
   txtCampo(0).Relation = ""
   txtCampo(0).Source = ""
   grdBrowse.AddColumn vgDb.Tables(vgIdentTab).Columns(txtCampo(0).DataField), txtCampo(0)

   Set txtCampo(1).CtPri = txtCp(1)
   txtCampo(1).DataType = 1
   txtCampo(1).Mask = "99999999"
   txtCampo(1).BoundColumn = ""
   txtCampo(1).ListFields = ""
   txtCampo(1).OrderFields = ""
   txtCampo(1).Relation = ""
   txtCampo(1).Source = ""
   grdBrowse.AddColumn vgDb.Tables(vgIdentTab).Columns(txtCampo(1).DataField), txtCampo(1)

   Set txtCampo(2).CtPri = txtCp(2)
   txtCampo(2).DataType = 0
   txtCampo(2).Mask = "@x"
   txtCampo(2).BoundColumn = ""
   txtCampo(2).ListFields = ""
   txtCampo(2).OrderFields = ""
   txtCampo(2).Relation = ""
   txtCampo(2).Source = ""
   grdBrowse.AddColumn vgDb.Tables(vgIdentTab).Columns(txtCampo(2).DataField), txtCampo(2)

   Set txtCampo(3).CtPri = txtCp(3)
   txtCampo(3).DataType = 1
   txtCampo(3).Mask = "999,9999"
   txtCampo(3).BoundColumn = ""
   txtCampo(3).ListFields = ""
   txtCampo(3).OrderFields = ""
   txtCampo(3).Relation = ""
   txtCampo(3).Source = ""
   grdBrowse.AddColumn vgDb.Tables(vgIdentTab).Columns(txtCampo(3).DataField), txtCampo(3)

   Set txtCampo(4).CtPri = txtCp(4)
   txtCampo(4).DataType = 1
   txtCampo(4).Mask = "999,9999"
   txtCampo(4).BoundColumn = ""
   txtCampo(4).ListFields = ""
   txtCampo(4).OrderFields = ""
   txtCampo(4).Relation = ""
   txtCampo(4).Source = ""
   grdBrowse.AddColumn vgDb.Tables(vgIdentTab).Columns(txtCampo(4).DataField), txtCampo(4)

   Set opcPainel1(0).CtPri = opcPainel1Cp(0)
   Set opcPainel1(0).CtFdo = labopcPainel1
   opcPainel1(0).DataType = 6
   opcPainel1(0).BookMark = 0
   grdBrowse.AddColumn vgDb.Tables(vgIdentTab).Columns(opcPainel1(0).DataField), opcPainel1(0)

   Set opcPainel1(1).CtPri = opcPainel1Cp(1)
   Set opcPainel1(1).CtFdo = labopcPainel1
   opcPainel1(1).DataType = 6
   opcPainel1(1).BookMark = 1

   Set chkCampo(0).CtPri = ChkCp(0)
   chkCampo(0).DataType = 5
   grdBrowse.AddColumn vgDb.Tables(vgIdentTab).Columns(chkCampo(0).DataField), chkCampo(0)

   Set chkCampo(1).CtPri = ChkCp(1)
   chkCampo(1).DataType = 5
   grdBrowse.AddColumn vgDb.Tables(vgIdentTab).Columns(chkCampo(1).DataField), chkCampo(1)

   Set chkCampo(2).CtPri = ChkCp(2)
   chkCampo(2).DataType = 5
   grdBrowse.AddColumn vgDb.Tables(vgIdentTab).Columns(chkCampo(2).DataField), chkCampo(2)

   Set chkCampo(3).CtPri = ChkCp(3)
   chkCampo(3).DataType = 5
   grdBrowse.AddColumn vgDb.Tables(vgIdentTab).Columns(chkCampo(3).DataField), chkCampo(3)

   Set txtCampo(5).CtPri = txtCp(5)
   txtCampo(5).DataType = 0
   txtCampo(5).Mask = "@x"
   txtCampo(5).BoundColumn = ""
   txtCampo(5).ListFields = ""
   txtCampo(5).OrderFields = ""
   txtCampo(5).Relation = ""
   txtCampo(5).Source = ""
   grdBrowse.AddColumn vgDb.Tables(vgIdentTab).Columns(txtCampo(5).DataField), txtCampo(5)

   Set opcPainel0(2).CtPri = opcPainel0Cp(2)
   Set opcPainel0(2).CtFdo = labopcPainel0
   opcPainel0(2).DataType = 6
   opcPainel0(2).BookMark = 2

   Set opcPainel0(0).CtPri = opcPainel0Cp(0)
   Set opcPainel0(0).CtFdo = labopcPainel0
   opcPainel0(0).DataType = 6
   opcPainel0(0).BookMark = 0
   grdBrowse.AddColumn vgDb.Tables(vgIdentTab).Columns(opcPainel0(0).DataField), opcPainel0(0)

   Set opcPainel0(1).CtPri = opcPainel0Cp(1)
   Set opcPainel0(1).CtFdo = labopcPainel0
   opcPainel0(1).DataType = 6
   opcPainel0(1).BookMark = 1

   Set txtCampo(6).CtPri = txtCp(6)
   txtCampo(6).DataType = 0
   txtCampo(6).Mask = "@x"
   txtCampo(6).BoundColumn = ""
   txtCampo(6).ListFields = ""
   txtCampo(6).OrderFields = ""
   txtCampo(6).Relation = ""
   txtCampo(6).Source = ""
   grdBrowse.AddColumn vgDb.Tables(vgIdentTab).Columns(txtCampo(6).DataField), txtCampo(6)

   ' Campo ClassTribId - Pesquisa na tabela ClassTrib (somente via botão P)
   ' NOTA: Não define DataField pois o campo mostra CodigoClassTrib mas salva o Id
   ' O DataField está vazio no design do controle txtCp(7)
   ' PesqFieldCapture = "CodigoClassTrib" para sempre retornar o código, não importa onde clicou
   Set txtCampo(7).CtPri = txtCp(7)
   Set txtCampo(7).CtFdo = labFdo7
   Set txtCampo(7).CtBot(BOT_LISTA) = bottxtCampo7(BOT_LISTA)
   bottxtCampo7(BOT_LISTA).Caption = "P"
   ' Nota: bottxtCampo7 só tem o índice BOT_LISTA (1), não tem BOT_COMBO (2)
   txtCampo(7).DataType = 0  ' Caractere (mostra código, não número)
   txtCampo(7).Mask = "@!"
   txtCampo(7).PesqModoAbertura = 2  ' Modo 2 = abre browse normal
   txtCampo(7).PesqFieldCapture = "CodigoClassTrib"  ' Sempre retorna o CodigoClassTrib
   txtCampo(7).BoundColumn = ""
   txtCampo(7).ListFields = ""
   txtCampo(7).ShowFields = ""
   txtCampo(7).OrderFields = ""
   txtCampo(7).Relation = ""
   txtCampo(7).Source = ""
   txtCampo(7).Filter = ""
   txtCampo(7).PesqSQLExpression = _
      "SELECT CodigoClassTrib, " & _
      "DescricaoClassTrib AS [Descrição], " & _
      "CodigoSituacaoTributaria AS [CST], " & _
      "TipoAliquota AS [Tipo Alíquota], " & _
      "PercentualReducaoIBS AS [Red.IBS %], " & _
      "PercentualReducaoCBS AS [Red.CBS %] " & _
      "FROM ClassTrib WHERE (Id > 0) ORDER BY CodigoClassTrib"

 Exit Sub

DeuErro:
  CErr.NumErro = Err
  CErr.FunctionName = "DefineControles0"
  CErr.Origem = CStr(vgFormID) + " - " + Me.Caption
 CErr.Show
End Sub

'Atualiza a descrição do ClassTrib no label de fundo
Private Sub AtualizaDescricaoClassTrib()
   AtualizaClassTribCompleto
End Sub

'Atualiza todos os campos do ClassTrib (descrição, redução IBS, CBS e tipo alíquota)
Private Sub AtualizaClassTribCompleto()
   Dim TbCT As GRecordSet
   Dim vgPV As Boolean
   Debug.Print ">>> AtualizaClassTribCompleto INICIO - ClassTribId=" & ClassTribId
   On Error Resume Next
   
   vgPV = vgPriVez
   vgPriVez = True  ' Evita recursividade
   
   ' Usa a variável ClassTribId que foi preenchida
   If ClassTribId > 0 Then
      Debug.Print ">>> AtualizaClassTribCompleto - vgDb Is Nothing = " & (vgDb Is Nothing)
      If vgDb Is Nothing Then
         vgPriVez = vgPV
         Exit Sub
      End If
      Debug.Print ">>> AtualizaClassTribCompleto - Abrindo recordset"
      Set TbCT = vgDb.OpenRecordSet("SELECT CodigoClassTrib, DescricaoClassTrib, CodigoSituacaoTributaria, " & _
         "PercentualReducaoIBS, PercentualReducaoCBS, TipoAliquota FROM ClassTrib WHERE Id = " & ClassTribId)
      Debug.Print ">>> AtualizaClassTribCompleto - TbCT Is Nothing = " & (TbCT Is Nothing)
      If Not TbCT Is Nothing Then
         Debug.Print ">>> AtualizaClassTribCompleto - RecordCount=" & TbCT.RecordCount
         If TbCT.RecordCount > 0 Then
            Debug.Print ">>> AtualizaClassTribCompleto - Atualizando campos"
            
            ' Atualiza label de descrição (60 caracteres para caber no labFdo7)
            labFdo7.Caption = TbCT!CodigoClassTrib & " - " & Left$(TbCT!DescricaoClassTrib & "", 60)
            labFdo7.ToolTipText = TbCT!CodigoClassTrib & " (CST: " & TbCT!CodigoSituacaoTributaria & ") - " & TbCT!DescricaoClassTrib
            
            ' Atualiza campos de redução (formato com 2 casas decimais)
            txtReducaoIBS.Text = Format$(IIf(IsNull(TbCT!PercentualReducaoIBS), 0, TbCT!PercentualReducaoIBS), "0.00") & "%"
            txtReducaoCBS.Text = Format$(IIf(IsNull(TbCT!PercentualReducaoCBS), 0, TbCT!PercentualReducaoCBS), "0.00") & "%"
            txtTipoAliquota.Text = IIf(IsNull(TbCT!TipoAliquota), "", TbCT!TipoAliquota) & ""
            
            ' Coloca o código no campo de texto (não o Id)
            txtCp(7).Text = TbCT!CodigoClassTrib & ""
         Else
            LimpaClassTribCampos
            txtCp(7).Text = ""
         End If
         Debug.Print ">>> AtualizaClassTribCompleto - Fechando recordset"
         TbCT.CloseRecordset
         Set TbCT = Nothing
      End If
   Else
      Debug.Print ">>> AtualizaClassTribCompleto - Limpando campos (ClassTribId=0)"
      LimpaClassTribCampos
      txtCp(7).Text = ""
   End If
   
   vgPriVez = vgPV
   Debug.Print ">>> AtualizaClassTribCompleto FIM"
   If Err Then Err.Clear
End Sub

'Limpa todos os campos relacionados ao ClassTrib
Private Sub LimpaClassTribCampos()
   On Error Resume Next
   ' NÃO limpa txtCp(7).Text aqui - isso é feito em lugares específicos
   labFdo7.Caption = ""
   labFdo7.ToolTipText = ""
   txtReducaoIBS.Text = ""
   txtReducaoCBS.Text = ""
   txtTipoAliquota.Text = ""
End Sub


'evento - antes de descarregar o formulário
Private Sub Form_QueryUnload(Cancel As Integer, UnloadMode As Integer)
   If vgSituacao <> ACAO_NAVEGANDO And vgBotoesOK Then  'botoeira esta correta?
      AtivaForm Me                                      'entao coloca
   End If
   Cancel = FormPendente(Me)                            've se tem atualizacao pendente
End Sub

'evento - redefinido o tamanho do formulário
Private Sub Form_Resize()
   AjustaPanFundo Me
End Sub

'evento - descarregando o formulário da memória
Private Sub Form_Unload(Cancel As Integer)
   Dim i As Integer
   On Error Resume Next
   FinalizaForm Me
   Set lblAjuste = Nothing
   For i = 0 To UBound(txtCampo)
      txtCampo(i).Finalize
      Set txtCampo(i) = Nothing
   Next
   Set chkCampo(0) = Nothing
   Set chkCampo(1) = Nothing
   Set chkCampo(2) = Nothing
   Set chkCampo(3) = Nothing

   'se tiver imprimindo registros em grade, fecha form de selecao/preview
   Unload vgFrmImpCons
   Set vgFrmImpCons = Nothing

   grdBrowse.Finalize                             'descarrega o grid
   Set frmClassifi = Nothing                      'libera o segmento de código do form
End Sub


'evento - quando o botão for apertado (ClassTrib)
Private Sub bottxtCampo7_Click(Index As Integer)
   Debug.Print ">>> bottxtCampo7_Click INICIO - Index=" & Index
   
   ' Apenas abre o BROWSE - o txtCp_Change vai cuidar da atualização
   txtCampo(7).BotClick Index
   
   Debug.Print ">>> bottxtCampo7_Click FIM"
End Sub

'evento - quando o mouse for pressionado sobre o campo
Private Sub txtCp_MouseDown(Index As Integer, Button As Integer, Shift As Integer, x As Single, y As Single)
   txtCampo(Index).MouseDown
End Sub


