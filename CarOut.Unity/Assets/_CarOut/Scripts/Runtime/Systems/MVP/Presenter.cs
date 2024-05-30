public abstract class Presenter
{
	protected Model Model;
	protected View View;

	protected Presenter(Model model, View view) 
	{
		Model = model;
		View = view;
	}
}
