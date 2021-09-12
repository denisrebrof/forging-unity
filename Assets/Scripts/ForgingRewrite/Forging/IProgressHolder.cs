using System;
namespace ForgingRefactoring
{
    public interface IProgressHolder
    {
        Action<float> OnProgressChanged{ get; }
        Action OnCompleted{ get; }
    }
}
