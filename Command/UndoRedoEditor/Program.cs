using System;
using System.Collections.Generic;

// Command Interface
public interface ICommand
{
  void Exectute();
  void UnExecute();
}

public class TextEditor
{
  public string Content { get; private set; } = "";
  public void Write(string text)
  {
    Content += text;
    Console.WriteLine($"Current content {Content}");
  }
  public void DeleteLast(int length)
  {
    if (Content.Length >= length)
    {
      Content = Content.Substring(0, Content.Length - length);
    }
    else
    {
      Content = "";
    }
    Console.WriteLine($"Current Content {Content}");
  }
}

public class WriteCommand : ICommand
{
  private TextEditor _editor;
  private string _text;

  public WriteCommand(TextEditor editor, string text)
  {
    _editor = editor;
    _text = text;
  }

  public void Exectute()
  {
    _editor.Write(_text);
  }
  public void UnExecute()
  {
    _editor.DeleteLast(_text.Length);
  }
}

public class DeleteCommand : ICommand
{
  private TextEditor _editor;
  private int _length;

  public DeleteCommand(TextEditor editor, int length)
  {
    _editor = editor;
    _length = length;
  }
  public void Exectute()
  {
    _editor.DeleteLast(_length);
  }
  public void UnExecute()
  {
    Console.WriteLine("Not So Simple");
  }

}

public class EditorInvoker
{
  private Stack<ICommand> _commandHistory = new Stack<ICommand>();
  private Stack<ICommand> _redoStack = new Stack<ICommand>();

  public void ExecuteCommand(ICommand command)
  {
    command.Exectute();
    _commandHistory.Push(command);
    _redoStack.Clear();
  }

  public void Undo()
  {
    if (_commandHistory.Count > 0)
    {
      ICommand command = _commandHistory.Pop();
      command.UnExecute();
      _redoStack.Push(command);
    }
    else
    {
      Console.WriteLine("Nothing to Undo");
    }
  }

  public void Redo()
  {
    if (_redoStack.Count > 0)
    {
      ICommand command = _redoStack.Pop();
      command.UnExecute();
      _commandHistory.Push(command);
    }
    else
    {
      Console.WriteLine("Nothing to Redo");
    }
  }
}

public class Prgoram
{
  public static void Main(string[] args)
  {
    TextEditor editor = new TextEditor();
    EditorInvoker invoker = new EditorInvoker();

    invoker.ExecuteCommand(new WriteCommand(editor, "Hello, "));
    invoker.ExecuteCommand(new WriteCommand(editor, "world!"));

    // Undo the last command (deletes "world!")
    invoker.Undo();

    // Redo the last undone command (re-adds "world!")
    invoker.Redo();
  }
}