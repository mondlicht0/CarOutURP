public abstract class Presenter
{
	private Model _model;
	private View _view;

	public Presenter(Model model, View view) 
	{
		_model = model;
		_view = view;
	}
}
