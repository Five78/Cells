public abstract class PersistentProperty<TPropertyType> : ObservableProperty<TPropertyType>
{
    protected TPropertyType _stored;

    private TPropertyType _defaultValue; 


    public PersistentProperty(TPropertyType defaultValue)
    {
        _defaultValue = defaultValue;
    }

    public override TPropertyType Value
    {
        get => _stored;
        set
        {
            var isEquels = _stored.Equals(value);
            if (isEquels) return;

            var oldValue = _value;
            Write(value);
            _stored = _value = value;

            InvokeChangedEvent(value, oldValue);
        }
    }

    public void Init()
    {
        _stored = _value = Read(_defaultValue);
    }

    protected abstract void Write(TPropertyType value);
    protected abstract TPropertyType Read(TPropertyType defaulValue);

    public void Validate()
    {
        if (!_stored.Equals(_value))
            Value = _value;
    }
}
