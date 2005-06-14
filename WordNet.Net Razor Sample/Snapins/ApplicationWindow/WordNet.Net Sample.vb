'/*
' * This file is a part of the WordNet.Net open source project.
' * 
' * Copyright (C) 2005 Malcolm Crowe, Troy Simpson 
' * 
' * Project Home: http://www.ebswift.com
' *
' * This library is free software; you can redistribute it and/or
' * modify it under the terms of the GNU Lesser General Public
' * License as published by the Free Software Foundation; either
' * version 2.1 of the License, or (at your option) any later version.
' *
' * This library is distributed in the hope that it will be useful,
' * but WITHOUT ANY WARRANTY; without even the implied warranty of
' * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
' * Lesser General Public License for more details.
' *
' * You should have received a copy of the GNU Lesser General Public
' * License along with this library; if not, write to the Free Software
' * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
' * 
' * */

Imports System
Imports System.Diagnostics
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.IO
Imports System.Reflection
Imports System.Windows.Forms
Imports Microsoft.VisualBasic
Imports Razor
Imports Razor.Attributes
Imports Razor.Configuration
Imports Razor.Features
Imports Razor.SnapIns
Imports Razor.SnapIns.WindowPositioningEngine

Namespace Razor.SnapIns.ApplicationWindow
	<SnapInTitle("WordNet.Net Sample Application"), _
	SnapInDescription("Demonstrates the capabilities of the WordNet.Net library built on top of the Razor engine."), _
	SnapInCompany("ebswift.com"), _
	SnapInDevelopers("Troy Simpson"), _
	SnapInVersion("1.0.0.0"), _
	SnapInDependency(GetType(WindowPositioningEngine.WindowPositioningEngineSnapIn))> _
	Public Class ApplicationWindowSnapIn
		Inherits SnapInWindow
		Implements IWindowPositioningEngineFeaturable
	
#Region " Razor Variables "
		Protected Shared _theInstance As ApplicationWindowSnapIn
		Protected _exitApplicationThreadOnClose As Boolean = True
		Protected _noPromptsOnClose As Boolean
		Public Const WindowPositioningEngineKey As String = "Main Window"
		Private components As System.ComponentModel.Container = Nothing
#End Region

#Region " Form Objects "
        Friend mnuFile As System.Windows.Forms.MenuItem
        Friend btnAdv As System.Windows.Forms.Button
        Friend btnNoun As System.Windows.Forms.Button
        WithEvents Friend Label3 As System.Windows.Forms.Label
        Public HelpMenuItem As System.Windows.Forms.MenuItem ' referenced by razor for autoupdate
        Friend btnAdj As System.Windows.Forms.Button
        Friend lblSearchInfo As System.Windows.Forms.Label
        WithEvents Friend MainMenu1 As System.Windows.Forms.MainMenu
        Friend mnuShowGloss As System.Windows.Forms.MenuItem
        Friend mnuExit As System.Windows.Forms.MenuItem
        Friend mnuLGPL As System.Windows.Forms.MenuItem
        Friend mnuOptions As System.Windows.Forms.MenuItem
        WithEvents Friend btnSearch As System.Windows.Forms.Button
        Friend mnuShowHelp As System.Windows.Forms.MenuItem
        Private txtOutput As System.Windows.Forms.TextBox
        Friend mnuWordWrap As System.Windows.Forms.MenuItem
        Friend mnuAdvancedOptions As System.Windows.Forms.MenuItem
        WithEvents Friend StatusBar1 As System.Windows.Forms.StatusBar
        Friend mnuSaveDisplay As System.Windows.Forms.MenuItem
        Friend txtSenses As System.Windows.Forms.TextBox
        WithEvents Friend Label1 As System.Windows.Forms.Label
        Friend txtSearchWord As System.Windows.Forms.TextBox
        Friend mnuWordNetLicense As System.Windows.Forms.MenuItem
        Friend btnOverview As System.Windows.Forms.Button
        Friend btnVerb As System.Windows.Forms.Button
        WithEvents Private mnuHistory As System.Windows.Forms.MenuItem
        Friend mnuClearDisplay As System.Windows.Forms.MenuItem
        WithEvents Friend SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
        WithEvents Friend MenuItem17 As System.Windows.Forms.MenuItem
#End Region

#Region " Form Variables "
        Private f3 As AdvancedOptions
        
        Private wnc As WordNetClasses.WN = New WordNetClasses.WN
        Private pbobject As Object = New Object
#End Region

#Region " Form Code "
		' Razor
		Public Shared ReadOnly Property Instance() As ApplicationWindowSnapIn
			Get
				Return _theInstance
			End Get
		End Property

		Public Sub New()
			MyBase.New()

			' *** Razor START
			_theInstance = Me
			Me.InitializeComponent
			If Not (SnapInHostingEngine.Instance.SplashWindow Is Nothing) Then
				AddHandler SnapInHostingEngine.Instance.SplashWindow.Closed, AddressOf OnSplashWindowClosed
			End If
			AddHandler Me.Start, AddressOf OnSnapInStart
			AddHandler Me.Stop, AddressOf OnSnapInStop
			AddHandler HelpMenuItem.Popup, AddressOf OnHelpMenuItemPopup
			' *** Razor End

            txtOutput.Anchor = Anchor.Top Or Anchor.Left Or Anchor.Bottom Or Anchor.Right
            f3 = New AdvancedOptions
            AddHandler mnuSaveDisplay.Click, AddressOf mnuSaveDisplay_Click
            AddHandler mnuClearDisplay.Click, AddressOf mnuClearDisplay_Click
            AddHandler mnuExit.Click, AddressOf mnuExit_Click
            AddHandler mnuWordWrap.Click, AddressOf mnuWordWrap_Click
            AddHandler mnuShowHelp.Click, AddressOf mnuShowHelp_Click
            AddHandler mnuShowGloss.Click, AddressOf mnuShowGloss_Click
            AddHandler mnuAdvancedOptions.Click, AddressOf mnuAdvancedOptions_Click
            AddHandler mnuWordNetLicense.Click, AddressOf mnuWordNetLicense_Click
            AddHandler mnuLGPL.Click, AddressOf mnuLGPL_Click
            AddHandler btnOverview.Click, AddressOf btnOverview_Click
            AddHandler btnNoun.Click, AddressOf btnWordType_Click
            AddHandler btnVerb.Click, AddressOf btnWordType_Click
            AddHandler btnAdj.Click, AddressOf btnWordType_Click
            AddHandler btnAdv.Click, AddressOf btnWordType_Click
		End Sub

		Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
			If disposing Then
				If Not (components Is Nothing) Then
					components.Dispose
				End If
			End If
			MyBase.Dispose(disposing)
		End Sub

		Private Sub InitializeComponent()
			Me.MenuItem17 = New System.Windows.Forms.MenuItem
			Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog
			Me.mnuClearDisplay = New System.Windows.Forms.MenuItem
			Me.mnuHistory = New System.Windows.Forms.MenuItem
			Me.btnVerb = New System.Windows.Forms.Button
			Me.btnOverview = New System.Windows.Forms.Button
			Me.mnuWordNetLicense = New System.Windows.Forms.MenuItem
			Me.txtSearchWord = New System.Windows.Forms.TextBox
			Me.Label1 = New System.Windows.Forms.Label
			Me.txtSenses = New System.Windows.Forms.TextBox
			Me.mnuSaveDisplay = New System.Windows.Forms.MenuItem
			Me.StatusBar1 = New System.Windows.Forms.StatusBar
			Me.mnuAdvancedOptions = New System.Windows.Forms.MenuItem
			Me.mnuWordWrap = New System.Windows.Forms.MenuItem
			Me.txtOutput = New System.Windows.Forms.TextBox
			Me.mnuShowHelp = New System.Windows.Forms.MenuItem
			Me.btnSearch = New System.Windows.Forms.Button
			Me.mnuOptions = New System.Windows.Forms.MenuItem
			Me.mnuLGPL = New System.Windows.Forms.MenuItem
			Me.mnuExit = New System.Windows.Forms.MenuItem
			Me.mnuShowGloss = New System.Windows.Forms.MenuItem
			Me.MainMenu1 = New System.Windows.Forms.MainMenu
			Me.lblSearchInfo = New System.Windows.Forms.Label
			Me.btnAdj = New System.Windows.Forms.Button
			Me.HelpMenuItem = New System.Windows.Forms.MenuItem
			Me.Label3 = New System.Windows.Forms.Label
			Me.btnNoun = New System.Windows.Forms.Button
			Me.btnAdv = New System.Windows.Forms.Button
			Me.mnuFile = New System.Windows.Forms.MenuItem
			Me.SuspendLayout
			'
			'MenuItem17
			'
			Me.MenuItem17.Index = 1
			Me.MenuItem17.Text = "-"
			'
			'SaveFileDialog1
			'
			Me.SaveFileDialog1.Filter = "Text files (*.txt)|*.txt"
			'
			'mnuClearDisplay
			'
			Me.mnuClearDisplay.Index = 1
			Me.mnuClearDisplay.Text = "Clear Current Display"
			'
			'mnuHistory
			'
			Me.mnuHistory.Index = -1
			Me.mnuHistory.Text = ""
			'
			'btnVerb
			'
			Me.btnVerb.FlatStyle = System.Windows.Forms.FlatStyle.System
			Me.btnVerb.Location = New System.Drawing.Point(352, 32)
			Me.btnVerb.Name = "btnVerb"
			Me.btnVerb.Size = New System.Drawing.Size(40, 23)
			Me.btnVerb.TabIndex = 4
			Me.btnVerb.Text = "Verb"
			Me.btnVerb.Visible = false
			'
			'btnOverview
			'
			Me.btnOverview.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
			Me.btnOverview.FlatStyle = System.Windows.Forms.FlatStyle.System
			Me.btnOverview.Location = New System.Drawing.Point(224, 32)
			Me.btnOverview.Name = "btnOverview"
			Me.btnOverview.TabIndex = 15
			Me.btnOverview.Text = "Overview"
			Me.btnOverview.Visible = false
			'
			'mnuWordNetLicense
			'
			Me.mnuWordNetLicense.Index = 0
			Me.mnuWordNetLicense.Text = "WordNet License (Princeton)"
			'
			'txtSearchWord
			'
			Me.txtSearchWord.AcceptsReturn = true
			Me.txtSearchWord.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
						Or System.Windows.Forms.AnchorStyles.Left)  _
						Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
			Me.txtSearchWord.Location = New System.Drawing.Point(80, 8)
			Me.txtSearchWord.Name = "txtSearchWord"
			Me.txtSearchWord.Size = New System.Drawing.Size(216, 20)
			Me.txtSearchWord.TabIndex = 1
			Me.txtSearchWord.Text = ""
			AddHandler Me.txtSearchWord.KeyDown, AddressOf Me.txtSearchWord_KeyDown
			'
			'Label1
			'
			Me.Label1.Location = New System.Drawing.Point(0, 8)
			Me.Label1.Name = "Label1"
			Me.Label1.Size = New System.Drawing.Size(80, 23)
			Me.Label1.TabIndex = 0
			Me.Label1.Text = "Search Word:"
			'
			'txtSenses
			'
			Me.txtSenses.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
			Me.txtSenses.Location = New System.Drawing.Point(544, 8)
			Me.txtSenses.Name = "txtSenses"
			Me.txtSenses.Size = New System.Drawing.Size(40, 20)
			Me.txtSenses.TabIndex = 8
			Me.txtSenses.Text = "0"
			'
			'mnuSaveDisplay
			'
			Me.mnuSaveDisplay.Index = 0
			Me.mnuSaveDisplay.Text = "Save Current Display"
			'
			'StatusBar1
			'
			Me.StatusBar1.Location = New System.Drawing.Point(0, 464)
			Me.StatusBar1.Name = "StatusBar1"
			Me.StatusBar1.Size = New System.Drawing.Size(592, 22)
			Me.StatusBar1.TabIndex = 9
			Me.StatusBar1.Text = "WordNet.Net Sample"
			'
			'mnuAdvancedOptions
			'
			Me.mnuAdvancedOptions.Index = 3
			Me.mnuAdvancedOptions.Text = "Advanced search options"
			'
			'mnuWordWrap
			'
			Me.mnuWordWrap.Checked = true
			Me.mnuWordWrap.Index = 0
			Me.mnuWordWrap.Text = "Word Wrap"
			'
			'txtOutput
			'
			Me.txtOutput.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
						Or System.Windows.Forms.AnchorStyles.Left)  _
						Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
			Me.txtOutput.Location = New System.Drawing.Point(0, 56)
			Me.txtOutput.Multiline = true
			Me.txtOutput.Name = "txtOutput"
			Me.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
			Me.txtOutput.Size = New System.Drawing.Size(592, 408)
			Me.txtOutput.TabIndex = 19
			Me.txtOutput.Text = "Licensed under the LGPL.  See http://opensource.ebswift.com/WordNet.Net for more "& _ 
"information"
			'
			'mnuShowHelp
			'
			Me.mnuShowHelp.Index = 1
			Me.mnuShowHelp.Text = "Show help with each search"
			'
			'btnSearch
			'
			Me.btnSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
			Me.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.System
			Me.btnSearch.Location = New System.Drawing.Point(304, 8)
			Me.btnSearch.Name = "btnSearch"
			Me.btnSearch.Size = New System.Drawing.Size(56, 23)
			Me.btnSearch.TabIndex = 13
			Me.btnSearch.Text = "Search"
			'
			'mnuOptions
			'
			Me.mnuOptions.Index = 1
			Me.mnuOptions.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuWordWrap, Me.mnuShowHelp, Me.mnuShowGloss, Me.mnuAdvancedOptions})
			Me.mnuOptions.Text = "Options"
			'
			'mnuLGPL
			'
			Me.mnuLGPL.Index = 2
			Me.mnuLGPL.Text = "License"
			'
			'mnuExit
			'
			Me.mnuExit.Index = 2
			Me.mnuExit.Text = "Exit"
			'
			'mnuShowGloss
			'
			Me.mnuShowGloss.Index = 2
			Me.mnuShowGloss.Text = "Show descriptive gloss"
			'
			'MainMenu1
			'
			Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFile, Me.mnuOptions, Me.HelpMenuItem})
			'
			'lblSearchInfo
			'
			Me.lblSearchInfo.Location = New System.Drawing.Point(0, 32)
			Me.lblSearchInfo.Name = "lblSearchInfo"
			Me.lblSearchInfo.Size = New System.Drawing.Size(296, 23)
			Me.lblSearchInfo.TabIndex = 2
			'
			'btnAdj
			'
			Me.btnAdj.FlatStyle = System.Windows.Forms.FlatStyle.System
			Me.btnAdj.Location = New System.Drawing.Point(400, 32)
			Me.btnAdj.Name = "btnAdj"
			Me.btnAdj.Size = New System.Drawing.Size(64, 23)
			Me.btnAdj.TabIndex = 5
			Me.btnAdj.Text = "Adjective"
			Me.btnAdj.Visible = false
			'
			'mnuHelp
			'
			Me.HelpMenuItem.Index = 2
			Me.HelpMenuItem.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuWordNetLicense, Me.MenuItem17, Me.mnuLGPL})
			Me.HelpMenuItem.Text = "Help"
			'
			'Label3
			'
			Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
			Me.Label3.Location = New System.Drawing.Point(480, 8)
			Me.Label3.Name = "Label3"
			Me.Label3.TabIndex = 7
			Me.Label3.Text = "Senses:"
			'
			'btnNoun
			'
			Me.btnNoun.FlatStyle = System.Windows.Forms.FlatStyle.System
			Me.btnNoun.Location = New System.Drawing.Point(304, 32)
			Me.btnNoun.Name = "btnNoun"
			Me.btnNoun.Size = New System.Drawing.Size(40, 23)
			Me.btnNoun.TabIndex = 3
			Me.btnNoun.Text = "Noun"
			Me.btnNoun.Visible = false
			'
			'btnAdv
			'
			Me.btnAdv.FlatStyle = System.Windows.Forms.FlatStyle.System
			Me.btnAdv.Location = New System.Drawing.Point(472, 32)
			Me.btnAdv.Name = "btnAdv"
			Me.btnAdv.Size = New System.Drawing.Size(48, 23)
			Me.btnAdv.TabIndex = 6
			Me.btnAdv.Text = "Adverb"
			Me.btnAdv.Visible = false
			'
			'mnuFile
			'
			Me.mnuFile.Index = 0
			Me.mnuFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuSaveDisplay, Me.mnuClearDisplay, Me.mnuExit})
			Me.mnuFile.Text = "File"
			'
			'StartForm
			'
			Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
			Me.ClientSize = New System.Drawing.Size(592, 486)
			Me.Controls.Add(Me.txtOutput)
			Me.Controls.Add(Me.btnOverview)
			Me.Controls.Add(Me.txtSenses)
			Me.Controls.Add(Me.txtSearchWord)
			Me.Controls.Add(Me.btnSearch)
			Me.Controls.Add(Me.StatusBar1)
			Me.Controls.Add(Me.Label3)
			Me.Controls.Add(Me.btnAdv)
			Me.Controls.Add(Me.btnAdj)
			Me.Controls.Add(Me.btnVerb)
			Me.Controls.Add(Me.btnNoun)
			Me.Controls.Add(Me.lblSearchInfo)
			Me.Controls.Add(Me.Label1)
			Me.Menu = Me.MainMenu1
			Me.Name = "StartForm"
			Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
			Me.Text = "WordNet.Net Sample"
			Me.ResumeLayout(false)
		End Sub
#End Region

#Region " Razor Code "
		Private Sub OnSnapInStart(ByVal sender As Object, ByVal e As EventArgs)
			Me.StartMyServices
		End Sub

		Private Sub OnSnapInStop(ByVal sender As Object, ByVal e As EventArgs)
			Me.StopMyServices
		End Sub

		Private Sub OnCommandLineReceivedFromAnotherInstance(ByVal sender As Object, ByVal e As ApplicationInstanceManagerEventArgs)
			Try
				WindowFlasher.FlashWindow(Me.Handle)
			Catch systemException As System.Exception
				System.Diagnostics.Trace.WriteLine(systemException)
			End Try
		End Sub

		Protected Overloads Overrides Sub StartMyServices()
			MyBase.StartMyServices
			Try
				Me.MorphAttributesToReflectStartingExecutable
				WindowPositioningEngine.WindowPositioningEngineSnapIn.Instance.Manage(Me, WindowPositioningEngineKey)
				AddHandler SnapInHostingEngine.GetExecutingInstance.InstanceManager.CommandLineReceivedFromAnotherInstance, AddressOf OnCommandLineReceivedFromAnotherInstance
				SnapInHostingEngine.Instance.ApplicationContext.AddTopLevelWindow(Me)
				Me.Show
				LoadAbout()
			Catch ex As Exception
				Trace.WriteLine(ex)
			End Try
		End Sub

		Protected Overloads Overrides Sub StopMyServices()
			MyBase.StopMyServices
			RemoveHandler SnapInHostingEngine.GetExecutingInstance.InstanceManager.CommandLineReceivedFromAnotherInstance, AddressOf OnCommandLineReceivedFromAnotherInstance
			Me.Close
		End Sub

		Public Property ExitApplicationThreadOnClose() As Boolean
			Get
				Return _exitApplicationThreadOnClose
			End Get
			Set
				_exitApplicationThreadOnClose = value
			End Set
		End Property

		Public Property NoPromptsOnClose() As Boolean
			Get
				Return _noPromptsOnClose
			End Get
			Set
				_noPromptsOnClose = value
			End Set
		End Property

		Protected Sub MorphAttributesToReflectStartingExecutable()
			Try
				Dim assembl As System.Reflection.Assembly = SnapInHostingEngine.GetExecutingInstance.StartingExecutable
				If Not (assembl Is Nothing) Then
					Dim filename As String = System.IO.Path.GetFileName(assembl.Location)
					filename = filename.Replace(System.IO.Path.GetExtension(assembl.Location), Nothing)
					Dim reader As AssemblyAttributeReader = New AssemblyAttributeReader(assembl)
					Dim companyAttributes As System.Reflection.AssemblyCompanyAttribute() = reader.GetAssemblyCompanyAttributes
					If Not (companyAttributes Is Nothing) Then
						If companyAttributes.Length > 0 Then
							If Not (companyAttributes(0).Company Is Nothing) AndAlso Not (companyAttributes(0).Company = String.Empty) Then
								Me.Text = filename
							End If
						End If
					End If
					Dim icon As Icon = ShellInformation.GetIconFromPath(assembl.Location, IconSizes.SmallIconSize, IconStyles.NormalIconStyle, FileAttributes.Normal)
					If Not (icon Is Nothing) Then
						Me.Icon = icon
					End If
				End If
			Catch systemException As System.Exception
				System.Diagnostics.Trace.WriteLine(systemException)
			End Try
		End Sub

		Public Function GetDefaultLocation() As Point Implements Razor.SnapIns.WindowPositioningEngine.IWindowPositioningEngineFeaturable.GetDefaultLocation
			Dim rc As Rectangle = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea
			Dim defSize As Size = Me.GetDefaultSize
			Dim x As Integer = (rc.Width / 2) - (defSize.Width / 2)
			Dim y As Integer = (rc.Height / 2) - (defSize.Height / 2)
			Return New Point(x, y)
		End Function

		Public Function GetDefaultSize() As Size Implements Razor.SnapIns.WindowPositioningEngine.IWindowPositioningEngineFeaturable.GetDefaultSize
			Return New Size(300, 300)
		End Function

		Public Function GetDefaultWindowState() As System.Windows.Forms.FormWindowState Implements Razor.SnapIns.WindowPositioningEngine.IWindowPositioningEngineFeaturable.GetDefaultWindowState
			Return FormWindowState.Normal
		End Function

		Private Sub OnSplashWindowClosed(ByVal sender As Object, ByVal e As EventArgs)
			Me.BringToFront
		End Sub

		Private Sub OnHelpMenuItemPopup(ByVal sender As Object, ByVal e As EventArgs)
			Dim mi As MenuItem = CType(sender, MenuItem)
			mi.MenuItems.Clear
			' this adds my non-razor options
			' autoupdate adds an autoupdate specific option
			mi.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuWordNetLicense, Me.MenuItem17, Me.mnuLGPL})
		End Sub
#End Region		

#Region " WordNet Sample Code "
        Private Sub LoadAbout()
            ' load the license text file
            Dim myFile As System.IO.StreamReader = New System.IO.StreamReader(MyPath() & "\license.txt")
            Dim mystring As String = myFile.ReadToEnd()

            myFile.Close()

            txtOutput.Text = mystring

            showFeedback(mystring)
        End Sub

        Private Function MyPath() As String
            'get the app path
            Dim fullAppName As String = [Assembly].GetExecutingAssembly().GetName().CodeBase
            'This strips off the exe name
            Dim FullAppPath As String = Path.GetDirectoryName(fullAppName)

            FullAppPath = Mid(FullAppPath, Len("file:\\"))

            ' following is only during testing
#If CONFIG = "Debug" Then
            FullAppPath = Mid(FullAppPath, 1, InStrRev(FullAppPath, "\"))
#End If


            Return FullAppPath
        End Function

        Private Sub Overview()
            'overview for 'search'
            Dim t As String
            Dim wnc As WordNetClasses.WN = New WordNetClasses.WN

            t = txtSearchWord.Text
            lblSearchInfo.Text = "Searches for " + t + ":"
            lblSearchInfo.Visible = True
            btnOverview.Visible = False

            txtOutput.Text = ""
            txtOutput.Visible = False
            StatusBar1.Text = "Overview of " + t
            Refresh()

            Try
                Dim b As Boolean ' sets the visibility of noun, verb, adj, adv when showing buttons for a word

                list = New ArrayList
                wnc.OverviewFor(t, "noun", b, bobj2, list)
                btnNoun.Visible = b

                wnc.OverviewFor(t, "verb", b, bobj3, list)
                btnVerb.Visible = b

                wnc.OverviewFor(t, "adj", b, bobj4, list)
                btnAdj.Visible = b

                wnc.OverviewFor(t, "adv", b, bobj5, list)
                btnAdv.Visible = b

                txtSearchWord.Text = t
                txtSenses.Text = "0"
            Catch ex As Exception
                MessageBox.Show(ex.Message & vbCrLf & vbCrLf & "Princeton's WordNet not pre-installed to default location?")
            End Try

            FixDisplay()
        End Sub

        Private Sub DoSearch(ByVal opt As Wnlib.Opt)
            If opt.sch.ptp.mnemonic = "OVERVIEW" Then
                Overview()
                Exit Sub
            End If

            txtOutput.Text = ""
            Refresh()

            list = New ArrayList
            Dim se As Wnlib.Search = New Wnlib.Search(txtSearchWord.Text, True, opt.pos, opt.sch, Int16.Parse(txtSenses.Text))
            Dim a As Integer = se.buf.IndexOf("\n")
            If (a >= 0) Then
                If (a = 0) Then
                    se.buf = se.buf.Substring(a + 1)
                    a = se.buf.IndexOf("\n")
                End If
                StatusBar1.Text = se.buf.Substring(0, a)
                se.buf = se.buf.Substring(a + 1)
            End If
            list.Add(se)
            If (Wnlib.WNOpt.opt("-h").flag) Then
                help = New Wnlib.WNHelp(opt.sch, opt.pos).help
            End If
            FixDisplay()
        End Sub

        Dim list As ArrayList = New ArrayList
        Dim help As String = ""

        Public Sub FixDisplay()
            pbobject = ""
            ShowResults()

            txtSearchWord.Focus()
        End Sub

        Private Sub ShowResults()
            Dim tmpstr As String = ""

            If list.Count = 0 Then
                showFeedback("Search for " & txtSearchWord.Text & " returned 0 results.")
                Exit Sub
            End If

            Dim tmptbox As Overview = New Overview

            If Not pbobject.GetType Is tmptbox.GetType Then
                Dim tb As Overview = New Overview
                txtOutput.Text = ""
                tb.useList(list, help, tmpstr)
                If Not help Is Nothing And help <> "" Then
                    tmpstr = help & vbcrlf & vbcrlf & tmpstr
                End If
                tmpstr = Replace(tmpstr, vbLf, vbCrLf)
                tmpstr = Replace(tmpstr, vbCrLf, "", 1, 1)
                tmpstr = Replace(tmpstr, "_", " ")
                showFeedback(tmpstr)

                If tmpstr = "" Or tmpstr = "<font color='green'><br />" & vbCr & " " & txtSearchWord.Text & " has no senses </font>" Then
                    showFeedback("Search for " & txtSearchWord.Text & " returned 0 results.")
                End If
                txtOutput.Visible = True
                pbobject = tb
            End If

            txtSearchWord.Focus()
        End Sub

        Public bobj2 As Wnlib.SearchSet
        Public bobj3 As Wnlib.SearchSet
        Public bobj4 As Wnlib.SearchSet
        Public bobj5 As Wnlib.SearchSet

        Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
            Overview()
            txtSearchWord.Focus()
        End Sub

        Private Sub txtSearchWord_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
            If e.KeyCode = Keys.Enter Then
                e.Handled = True
                btnSearch_Click(Nothing, Nothing)
            End If
        End Sub

        Dim opts As ArrayList = Nothing

        Private Sub btnWordType_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Dim b As Button = sender
            Dim ss As Wnlib.SearchSet
            Dim btext As String = b.Text

            If btext = "Adjective" Then
                btext = "Adj"
            End If

            Select Case btext
                Case "Noun"
                    ss = CType(bobj2, Wnlib.SearchSet)

                Case "Verb"
                    ss = CType(bobj3, Wnlib.SearchSet)

                Case "Adj"
                    ss = CType(bobj4, Wnlib.SearchSet)

                Case "Adverb"
                    ss = CType(bobj5, Wnlib.SearchSet)
            End Select
            
            Dim pos As Wnlib.PartOfSpeech = Wnlib.PartOfSpeech.of(btext.ToLower)
            Dim i As Integer
            opts = New ArrayList
            Dim cm As ContextMenu = New ContextMenu
            Dim tmplst As ArrayList = New ArrayList

            For i = 0 To Wnlib.Opt.Count - 1
                Dim opt As Wnlib.Opt = opt.at(i)

                If ss(opt.sch.ptp.ident) And opt.pos Is pos Then
                    If tmplst.IndexOf(opt.label) = -1 And opt.label <> "Grep" Then
                        Dim mi As MenuItem = New MenuItem
                        mi.Text = opt.label
                        AddHandler mi.Click, AddressOf searchMenu_Click
                        opts.Add(opt)
                        cm.MenuItems.Add(mi)

                        tmplst.Add(opt.label)
                    End If
                End If
            Next i
            cm.Show(b.Parent, New Point(sender.left, b.Bottom))
        End Sub

        Private Sub searchMenu_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            ' one of the options for button2_click was selected
            Dim mi As MenuItem = sender
            Dim opt As Wnlib.Opt
            Dim i As Integer
            Dim tmpstr As String

            txtOutput.Text = ""
            tmpstr = mi.Text
            tmpstr = Replace(tmpstr, "Syns", "Synonyms")
            tmpstr = Replace(tmpstr, " x ", " by ")
            tmpstr = Replace(tmpstr, "Freq:", "Frequency:")
            StatusBar1.Text = tmpstr
            Refresh()

            For i = 0 To mi.Parent.MenuItems.Count - 1
                If mi.Text = mi.Parent.MenuItems(i).Text Then
                    opt = opts(i)
                End If
            Next i
            DoSearch(opt)
            btnOverview.Visible = True

            Refresh()
        End Sub

        Private Sub mnuWordWrap_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
            sender.checked = Not sender.checked

			txtOutput.WordWrap = sender.checked
            showFeedback(txtOutput.Text)
        End Sub

        Private Sub mnuShowHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        	sender.Checked = (Not sender.Checked)
            Wnlib.WNOpt.opt("-h").flag = sender.Checked
        End Sub

        Private Sub mnuShowGloss_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
            sender.Checked = Not sender.Checked
            Wnlib.WNOpt.opt("-g").flag = Not sender.Checked
        End Sub

        Private Sub btnOverview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Overview()
        End Sub

        Private Sub mnuExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Application.Exit()
        End Sub

        Private Sub mnuClearDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        	txtOutput.Text = ""
            txtSearchWord.Text = ""
            lblSearchInfo.Text = ""
            StatusBar1.Text = "WordNetDT"
            btnNoun.Visible = False
            btnVerb.Visible = False
            btnAdj.Visible = False
            btnAdv.Visible = False
            btnOverview.Visible = False
            btnSearch.Visible = True
        End Sub

        Private Sub mnuWordNetLicense_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Dim myFile As System.IO.StreamReader = New System.IO.StreamReader(MyPath() & "\wordnetlicense.txt")
            Dim mystring As String = myFile.ReadToEnd()

            myFile.Close()

            showFeedback(mystring)
        End Sub

        Private Sub mnuLGPL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
            LoadAbout()
        End Sub

        Private Sub mnuSaveDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
            SaveFileDialog1.FileName = txtSearchWord.Text
            If (SaveFileDialog1.ShowDialog() = DialogResult.OK) Then
                Dim f As StreamWriter = New StreamWriter(SaveFileDialog1.FileName, False)

                f.Write(txtOutput.Text)
                f.Close()
            End If
        End Sub

        Private Sub showFeedback(ByVal mystring As String)
			txtOutput.Text = mystring
            txtSearchWord.Focus()
        End Sub

	    Private Sub mnuAdvancedOptions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
	        f3.ShowDialog()
	    End Sub
#End Region
    End Class

#Region " WordNet Sample Code - continued "
	Public Class Overview
		' really basic thrown together class which makes the plain text search results
		Private cont As ArrayList
		Private totLines As Integer
		Private sw As String
		Private helpLines As Integer
			
		Sub usePassage(ByVal passage As String, ByRef tmpstr As String)
			tmpstr += passage
		End Sub
	
		Public Sub useList(ByVal w As ArrayList, ByVal help As String, ByRef tmpstr As String)
			cont = New ArrayList
			totLines = 0
			sw = Nothing
	
			If Not (help Is Nothing) AndAlso Not (help = "") Then
				usePassage(help, tmpstr)
			End If
			helpLines = totLines
			Dim j As Integer = 0
			While j < w.Count
				Dim se As Wnlib.Search = CType(w(j), Wnlib.Search)
				sw = se.word
				usePassage(se.buf, tmpstr)
				System.Math.Min(System.Threading.Interlocked.Increment(j),j-1)
			End While
		End Sub
	End Class
#End Region
End Namespace
