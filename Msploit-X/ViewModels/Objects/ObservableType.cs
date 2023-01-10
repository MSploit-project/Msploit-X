using ReactiveUI;

namespace Msploit_X.ViewModels.Objects
{
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
            this.Value = value;
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