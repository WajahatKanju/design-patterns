using System;
using System.Collections.Generic;

public interface ICommand
{
  void Execute();
  void UnExecute();
}

public class TextEditor
{
  public string Content { get; private set; } = "";

  public void Write(string text)
  {
    Content += text;
    Console.WriteLine($"Current content: {Content}");
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
    Console.WriteLine($"Current Content: {Content}");
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

  public void Execute()
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
  private string _deletedText;

  public DeleteCommand(TextEditor editor, int length)
  {
    _editor = editor;
    _deletedText = editor.Content.Substring(editor.Content.Length - length);
  }

  public void Execute()
  {
    _editor.DeleteLast(_deletedText.Length);
  }

  public void UnExecute()
  {
    _editor.Write(_deletedText);
  }
}

public class EditorInvoker
{
  private Stack<ICommand> _commandHistory = new Stack<ICommand>();
  private Stack<ICommand> _redoStack = new Stack<ICommand>();

  public void ExecuteCommand(ICommand command)
  {
    command.Execute();
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
      command.Execute();
      _commandHistory.Push(command);
    }
    else
    {
      Console.WriteLine("Nothing to Redo");
    }
  }
}

public class Program
{
  public static void Main(string[] args)
  {
    TextEditor editor = new TextEditor();
    EditorInvoker invoker = new EditorInvoker();

    invoker.ExecuteCommand(new WriteCommand(editor, "Hello, "));
    invoker.ExecuteCommand(new WriteCommand(editor, "world!"));

    invoker.Undo();
    invoker.Redo();
  }
}
