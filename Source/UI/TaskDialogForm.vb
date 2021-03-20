﻿
Imports StaxRip.UI

Public Class TaskDialogForm
    Class TaskDialogPanel
        Inherits Panel

        Protected Overrides Sub OnLayout(levent As LayoutEventArgs)
            If DesignMode OrElse Controls.Count = 0 Then
                MyBase.OnLayout(levent)
                Exit Sub
            End If

            Dim fh = FontHeight
            Dim previous As Control

            Using g = CreateGraphics()
                For x = 0 To Controls.Count - 1
                    Dim c = Controls(x)

                    If x <> 0 Then
                        c.Top = previous.Top + previous.Height + 1
                    End If

                    c.Left = CInt(fh * 0.7)
                    c.Width = ClientSize.Width - CInt(fh * 0.7 * 2)

                    If TypeOf c Is TextBox Then
                        Dim sz = g.MeasureString(c.Text, c.Font, c.ClientSize.Width)
                        c.Height = CInt(sz.Height)
                    End If

                    TryCast(c, CommandButton)?.AdjustSize()

                    previous = c
                Next
            End Using
        End Sub
    End Class

    Class CommandButton
        Inherits ButtonEx

        Property Title As String
        Property Description As String

        Property TitleFont As Font = New Font("Segoe UI", 12)
        Property DescriptionFont As Font = New Font("Segoe UI", 9)

        Sub AdjustSize()
            Dim tf = TitleFont.Height
            Dim h As Integer

            If Title <> "" AndAlso Description <> "" Then
                Dim ts = GetTitleSize()
                Dim ds = GetDescriptionSize()
                h = CInt(tf * 0.2 * 2) + ts.Height + ds.Height
            ElseIf Title <> "" Then
                Dim ts = GetTitleSize()
                h = CInt(tf * 0.2 * 2) + ts.Height
            ElseIf Description <> "" Then
                Dim ds = GetDescriptionSize()
                h = CInt(tf * 0.2 * 2) + ds.Height
            End If

            ClientSize = New Size(ClientSize.Width, h)
        End Sub

        Function GetTitleSize(Optional g1 As Graphics = Nothing) As Size
            If Title = "" Then
                Exit Function
            End If

            Dim g2 = g1

            If g2 Is Nothing Then
                g2 = CreateGraphics()
            End If

            Dim tf = TitleFont.Height
            Dim w = ClientSize.Width - CInt(tf * 0.3 * 2)
            Dim sz = g2.MeasureString(Title, TitleFont, w)

            If g1 Is Nothing Then
                g2.Dispose()
            End If

            Return New Size(CInt(sz.Width), CInt(sz.Height))
        End Function

        Function GetDescriptionSize(Optional g1 As Graphics = Nothing) As Size
            If Description = "" Then
                Exit Function
            End If

            Dim g2 = g1

            If g2 Is Nothing Then
                g2 = CreateGraphics()
            End If

            Dim tf = TitleFont.Height
            Dim w = ClientSize.Width - CInt(tf * 0.3 * 2)
            Dim sz = g2.MeasureString(Description, Font, w)

            If g1 Is Nothing Then
                g2.Dispose()
            End If

            Return New Size(CInt(sz.Width), CInt(sz.Height))
        End Function

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)

            Dim g = e.Graphics
            Dim titleFontHeight = TitleFont.Height
            Dim x = CInt(titleFontHeight * 0.3)
            Dim y = CInt(titleFontHeight * 0.2)
            Dim w = ClientSize.Width - x * 2
            Dim h = ClientSize.Height - CInt(titleFontHeight * 0.2 * 2)
            Dim r = New Rectangle(x, y, w, h)

            If Title <> "" AndAlso Description <> "" Then
                g.DrawString(Title, TitleFont, Brushes.White, r)
                y = CInt(titleFontHeight * 0.2) + GetTitleSize().Height
                r = New Rectangle(x, y, w, h)
                g.DrawString(Description, Font, Brushes.White, r)
            ElseIf Title <> "" Then
                g.DrawString(Title, TitleFont, Brushes.White, r)
            ElseIf Description <> "" Then
                g.DrawString(Description, Font, Brushes.White, r)
            End If
        End Sub

        Protected Overrides Sub Dispose(disposing As Boolean)
            MyBase.Dispose(disposing)
            TitleFont.Dispose()
        End Sub
    End Class
End Class
