Imports Windows.Win32.System

Public Class Form1
    Dim SudokuArray(81) As TextBox
    Dim myNumber As Integer = 1
    Dim SelectedCell As Integer
    Dim SudokuDisplay(81) As Integer
    Dim SudokuAnswer(81) As Integer
    Dim Answer As String
    Dim solvedSudokuIndices As String
    Dim Game As String
    Dim Game1 As String = ""
    Dim Life = 10
    Dim Blank = 0
    Private rand As New Random()
    Dim elapsedTime As Integer = 0
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        ' Increment the elapsed time and update the time display
        elapsedTime += 1
        UpdateTimeDisplay()
    End Sub
    Private Sub UpdateTimeDisplay()
        ' Update the time display label with the elapsed time
        Dim hours As Integer = elapsedTime \ 3600
        Dim minutes As Integer = (elapsedTime Mod 3600) \ 60
        Dim seconds As Integer = elapsedTime Mod 60
        Label5.Text = $"{hours:00}:{minutes:00}:{seconds:00}"
    End Sub
    Private Sub PrintSudoku(grid As Integer(,))
        For row As Integer = 0 To 8
            For col As Integer = 0 To 8
                Console.Write(grid(row, col) & " ")
            Next
            Console.WriteLine()
        Next
    End Sub
    Private Function StringToGrid(indices As String) As Integer(,)
        Dim grid(8, 8) As Integer
        Dim index As Integer = 0
        For row As Integer = 0 To 8
            For col As Integer = 0 To 8
                grid(row, col) = Integer.Parse(indices(index))
                index += 1
            Next
        Next
        Return grid
    End Function
    Private Function IsValid(grid As Integer(,), row As Integer, col As Integer, num As Integer) As Boolean
        ' Check if the number exists in the same row or column
        For i As Integer = 0 To 8
            If grid(row, i) = num OrElse grid(i, col) = num Then
                Return False
            End If
        Next

        ' Check if the number exists in the 3x3 subgrid
        Dim subgridRow As Integer = row - row Mod 3
        Dim subgridCol As Integer = col - col Mod 3
        For i As Integer = 0 To 2
            For j As Integer = 0 To 2
                If grid(subgridRow + i, subgridCol + j) = num Then
                    Return False
                End If
            Next
        Next

        Return True
    End Function

    ' Function to solve the Sudoku grid using backtracking
    Private Function SolveSudoku(grid As Integer(,)) As Boolean
        Dim random As New Random()

        For row As Integer = 0 To 8
            For col As Integer = 0 To 8
                If grid(row, col) = 0 Then
                    Dim numbersToTry As New List(Of Integer)() From {1, 2, 3, 4, 5, 6, 7, 8, 9}
                    numbersToTry = numbersToTry.OrderBy(Function(x) random.Next()).ToList()

                    For Each num As Integer In numbersToTry
                        If IsValid(grid, row, col, num) Then
                            grid(row, col) = num
                            If SolveSudoku(grid) Then
                                Return True
                            End If
                            grid(row, col) = 0
                        End If
                    Next
                    Return False
                End If
            Next
        Next
        Return True

    End Function


    Private Function GenerateSudoku() As String
        Dim grid(8, 8) As Integer
        SolveSudoku(grid)
        ' Remove random numbers to create a puzzle
        Dim count As Integer = 30 ' Adjust the count to change the difficulty level
        Do While count > 0
            Dim row As Integer = rand.Next(9)
            Dim col As Integer = rand.Next(9)
            If grid(row, col) <> 0 Then
                grid(row, col) = 0
                count -= 1
            End If
        Loop
        'Dim Game1 As String = ""
        For row As Integer = 0 To 8
            For col As Integer = 0 To 8
                Game1 &= grid(row, col).ToString()
            Next
        Next

        ' Convert the grid into a single string
        Return GridToString(grid)
    End Function

    Private Function GridToString(grid As Integer(,)) As String
        Dim indices As String = ""
        'indices &= 
        For row As Integer = 0 To 8
            For col As Integer = 0 To 8
                indices &= grid(row, col).ToString()
            Next
        Next
        Return indices
    End Function
    Private Sub GenerateNewGame()
        Dim grid(8, 8) As Integer
        SolveSudoku(grid)
        Dim sudokuIndices As String = GenerateSudoku()

        ' Convert the single string into a Sudoku grid
        Dim sudokuGrid As Integer(,) = StringToGrid(sudokuIndices)

        ' Solve the Sudoku puzzle
        If SolveSudoku(sudokuGrid) Then
            ' Convert the solved Sudoku grid into a single string
            solvedSudokuIndices = GridToString(sudokuGrid)

            'Answer = solvedSudokuIndices
            ' Display the solved Sudoku string
            Dim temp As String = ""
            temp += "_"
            temp += solvedSudokuIndices
            Answer = temp
            temp = ""
            temp += "_"
            temp += Game1
            Game = temp
            solvedSudokuIndices = ""
            Game1 = ""
            'MessageBox.Show("Solved Sudoku grid indices: " & temp)
        End If
    End Sub
    Public Sub LoadGame()

        Timer1.Start()
        'Dim Game As String
        'Game = "_350627000069803207000504010730900561800000400600001300020309045000075800573200006"
        'Answer = "_351627984469813257287594613734982561815736429692451378128369745946175832573248196"
        'MsgBox(Answer)
        Dim Index As Integer
        For Index = 1 To 81
            SudokuArray(Index).Tag = Index
            SudokuAnswer(Index) = Val(Answer.Substring(Index, 1))
            SudokuDisplay(Index) = Val(Game.Substring(Index, 1))
            If SudokuDisplay(Index) <> 0 Then
                SudokuArray(Index).Text = SudokuDisplay(Index).ToString
                SudokuArray(Index).Font = New Font("Verdana", 10, FontStyle.Bold)
                SudokuArray(Index).Enabled = False
            Else
                Blank += 1
            End If
        Next
        UpdateTimeDisplay()
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'Button1.PerformClick()
        Timer1.Stop()
        elapsedTime = 0
        Dim Index As Integer
        For Index = 1 To 81
            SudokuArray(Index).Text = ""
            SudokuArray(Index).Font = New Font("Microsoft Sans Serif", 8.25, FontStyle.Regular)
            SudokuArray(Index).BackColor = Color.White
        Next
        Life = 10
        Label3.Text = Life

        LoadGame()


    End Sub

    Public Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Dim grid(8, 8) As Integer
        Timer1.Stop()
        elapsedTime = 0
        For Index = 1 To 81
            SudokuArray(Index).Text = ""
            SudokuArray(Index).Font = New Font("Microsoft Sans Serif", 8.25, FontStyle.Regular)
            SudokuArray(Index).BackColor = Color.White
            SudokuArray(Index).Enabled = True

        Next
        Answer = ""
        Game = ""

        GenerateNewGame()
        LoadGame()
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        SudokuArray(1) = TextBox1
        SudokuArray(2) = TextBox2
        SudokuArray(3) = TextBox3
        SudokuArray(4) = TextBox4
        SudokuArray(5) = TextBox5
        SudokuArray(6) = TextBox6
        SudokuArray(7) = TextBox7
        SudokuArray(8) = TextBox8
        SudokuArray(9) = TextBox9
        SudokuArray(10) = TextBox10
        SudokuArray(11) = TextBox11
        SudokuArray(12) = TextBox12
        SudokuArray(13) = TextBox13
        SudokuArray(14) = TextBox14
        SudokuArray(15) = TextBox15
        SudokuArray(16) = TextBox16
        SudokuArray(17) = TextBox17
        SudokuArray(18) = TextBox18
        SudokuArray(19) = TextBox19
        SudokuArray(20) = TextBox20
        SudokuArray(21) = TextBox21
        SudokuArray(22) = TextBox22
        SudokuArray(23) = TextBox23
        SudokuArray(24) = TextBox24
        SudokuArray(25) = TextBox25
        SudokuArray(26) = TextBox26
        SudokuArray(27) = TextBox27
        SudokuArray(28) = TextBox28
        SudokuArray(29) = TextBox29
        SudokuArray(30) = TextBox30
        SudokuArray(31) = TextBox31
        SudokuArray(32) = TextBox32
        SudokuArray(33) = TextBox33
        SudokuArray(34) = TextBox34
        SudokuArray(35) = TextBox35
        SudokuArray(36) = TextBox36
        SudokuArray(37) = TextBox37
        SudokuArray(38) = TextBox38
        SudokuArray(39) = TextBox39
        SudokuArray(40) = TextBox40
        SudokuArray(41) = TextBox41
        SudokuArray(42) = TextBox42
        SudokuArray(43) = TextBox43
        SudokuArray(44) = TextBox44
        SudokuArray(45) = TextBox45
        SudokuArray(46) = TextBox46
        SudokuArray(47) = TextBox47
        SudokuArray(48) = TextBox48
        SudokuArray(49) = TextBox49
        SudokuArray(50) = TextBox50
        SudokuArray(51) = TextBox51
        SudokuArray(52) = TextBox52
        SudokuArray(53) = TextBox53
        SudokuArray(54) = TextBox54
        SudokuArray(55) = TextBox55
        SudokuArray(56) = TextBox56
        SudokuArray(57) = TextBox57
        SudokuArray(58) = TextBox58
        SudokuArray(59) = TextBox59
        SudokuArray(60) = TextBox60
        SudokuArray(61) = TextBox61
        SudokuArray(62) = TextBox62
        SudokuArray(63) = TextBox63
        SudokuArray(64) = TextBox64
        SudokuArray(65) = TextBox65
        SudokuArray(66) = TextBox66
        SudokuArray(67) = TextBox67
        SudokuArray(68) = TextBox68
        SudokuArray(69) = TextBox69
        SudokuArray(70) = TextBox70
        SudokuArray(71) = TextBox71
        SudokuArray(72) = TextBox72
        SudokuArray(73) = TextBox73
        SudokuArray(74) = TextBox74
        SudokuArray(75) = TextBox75
        SudokuArray(76) = TextBox76
        SudokuArray(77) = TextBox77
        SudokuArray(78) = TextBox78
        SudokuArray(79) = TextBox79
        SudokuArray(80) = TextBox80
        SudokuArray(81) = TextBox81
        'LoadGame()
        'For Index As Integer = 1 To 81
        'SudokuArray(Index).Tag = Index
        'Next
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Close()
    End Sub

    Private Sub ClickArray(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.Click, TextBox2.Click, TextBox3.Click, TextBox4.Click, TextBox5.Click, TextBox6.Click, TextBox7.Click, TextBox8.Click, TextBox9.Click, TextBox10.Click, TextBox11.Click, TextBox12.Click, TextBox13.Click, TextBox14.Click, TextBox15.Click, TextBox16.Click, TextBox17.Click, TextBox18.Click, TextBox19.Click, TextBox20.Click, TextBox21.Click, TextBox22.Click, TextBox23.Click, TextBox24.Click, TextBox25.Click, TextBox26.Click, TextBox27.Click, TextBox28.Click, TextBox29.Click, TextBox30.Click, TextBox31.Click, TextBox32.Click, TextBox33.Click, TextBox34.Click, TextBox35.Click, TextBox36.Click, TextBox37.Click, TextBox38.Click, TextBox39.Click, TextBox40.Click, TextBox41.Click, TextBox42.Click, TextBox43.Click, TextBox44.Click, TextBox45.Click, TextBox46.Click, TextBox47.Click, TextBox48.Click, TextBox49.Click, TextBox50.Click, TextBox51.Click, TextBox52.Click, TextBox53.Click, TextBox54.Click, TextBox55.Click, TextBox56.Click, TextBox57.Click, TextBox58.Click, TextBox59.Click, TextBox60.Click, TextBox61.Click, TextBox62.Click, TextBox63.Click, TextBox64.Click, TextBox65.Click, TextBox66.Click, TextBox67.Click, TextBox68.Click, TextBox69.Click, TextBox70.Click, TextBox71.Click, TextBox72.Click, TextBox73.Click, TextBox74.Click, TextBox75.Click, TextBox76.Click, TextBox77.Click, TextBox78.Click, TextBox79.Click, TextBox80.Click, TextBox81.Click
        Dim tempTextBox As TextBox
        tempTextBox = CType(sender, TextBox)
        SelectedCell = Val(tempTextBox.Tag)
        SudokuArray(SelectedCell).BackColor = Color.AliceBlue

    End Sub
    Sub ShadeRow()
        SudokuArray(SelectedCell).BackColor = Color.AliceBlue
        For Index As Integer = (((SelectedCell \ 9) * 9) + 1) To ((SelectedCell \ 9) * 9) + 9
            SudokuArray(Index).BackColor = Color.Beige
        Next
    End Sub
    Sub ShadeCube()
        Dim RowNumber = (SelectedCell - 1) \ 9
        RowNumber = (RowNumber \ 3) * 3
        Dim ColNumber = (SelectedCell - 1) Mod 9
        ColNumber = (ColNumber \ 3) * 3

        For RowOffset As Integer = 0 To 2
            For ColOffset As Integer = 0 To 2
                Dim cellIndex = ((RowNumber + RowOffset) * 9) + (ColNumber + ColOffset + 1)
                If cellIndex >= 0 AndAlso cellIndex < 82 Then
                    SudokuArray(cellIndex).BackColor = Color.Bisque
                End If
            Next
        Next
    End Sub



    Sub ColorColumnOfSelectedCell(ByVal selectedCellIndex As Integer, ByVal color As Color)
        ' Calculate the column index of the selected cell
        Dim columnIndex As Integer = (selectedCellIndex - 1) Mod 9 + 1

        ' Iterate through each cell in the column and color it
        For row As Integer = 0 To 8
            Dim cellIndex As Integer = columnIndex + (row * 9)
            SudokuArray(cellIndex).BackColor = color
        Next
    End Sub
    Private Sub Button_Click(sender As Object, e As EventArgs) Handles Button4.Click, Button5.Click, Button6.Click, Button7.Click, Button8.Click, Button9.Click, Button10.Click, Button11.Click, Button12.Click
        Dim btn As Button
        Dim Variable As Integer
        btn = CType(sender, Button)
        Dim Display As Integer = Val(btn.Tag)
        Variable = SudokuArray(SelectedCell).Tag
        Val(btn.Tag)
        If Val(btn.Tag) = Answer.Substring(Variable, 1) Then
            SudokuArray(SelectedCell).Text = Val(btn.Tag)
            Blank -= 1
            If Blank = 0 Then
                MsgBox("Congratulations!")
                Timer1.Stop()

            End If
        Else
            SudokuArray(SelectedCell).Text = Val(btn.Tag)
            MsgBox("Wrong!")
            SudokuArray(SelectedCell).Text = ""
            Life -= 1
            Label3.Text = Life
            If Life = 0 Then
                Label3.ForeColor = Color.Red
                Label1.BackColor = Color.Red
                MsgBox("You Failed!")
                Button1.PerformClick()
            End If
        End If

        ShadeRow()
        ShadeCube()
        ColorColumnOfSelectedCell(SelectedCell, Color.Beige)
    End Sub

    Private Sub LabelChange(sender As Object, e As EventArgs) Handles Me.Load
        Label1.Text = "Hearts"
        'Label1.Size = New Size(112, 34)
        Label1.BackColor = Color.Green
    End Sub

    Public Sub Label3_Click(sender As Object, e As EventArgs) Handles Me.Load
        Label3.Text = Life
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        Dim Index As Integer
        For Index = 1 To 81
            SudokuArray(Index).Tag = Index
            SudokuAnswer(Index) = Val(Answer.Substring(Index, 1))
            SudokuDisplay(Index) = Val(Answer.Substring(Index, 1))
            If SudokuDisplay(Index) <> 0 Then
                SudokuArray(Index).Text = SudokuDisplay(Index).ToString
                SudokuArray(Index).Font = New Font("Verdana", 10, FontStyle.Bold)
                SudokuArray(Index).Enabled = False
            Else
                Blank += 1
            End If
        Next
    End Sub
End Class
