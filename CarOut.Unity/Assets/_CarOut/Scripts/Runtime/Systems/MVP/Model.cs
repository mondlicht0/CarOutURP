public abstract class Model
{
	// Config and State
	
	protected ModelConfig _config;

	protected View _view;

	// protected List<EState> _states; EState - Enum States

	public Model(View view) 
	{
		_view = view;
	}
}
