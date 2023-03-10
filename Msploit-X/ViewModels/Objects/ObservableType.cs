using System.Reflection;
using ReactiveUI;

namespace Msploit_X.ViewModels.Objects
{
    [Obfuscation(Exclude = true)]
    public class ObservableType<T> : ReactiveObject
    {
        private T valueB;
        public T Value
        {
            get => valueB;
            set => this.RaiseAndSetIfChanged(ref valueB, value);
        }
        public ObservableType(T value)
        {
            Value = value;
        }

        public ObservableType()
        {
            Value = default;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override bool Equals(object? obj)
        {
            return Value.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}