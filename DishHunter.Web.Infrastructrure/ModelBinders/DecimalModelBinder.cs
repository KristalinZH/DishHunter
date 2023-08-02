namespace DishHunter.Web.Infrastructrure.ModelBinders
{
	using Microsoft.AspNetCore.Mvc.ModelBinding;
	using System.Globalization;
	using System.Threading.Tasks;

	public class DecimalModelBinder : IModelBinder
	{
		public Task BindModelAsync(ModelBindingContext bindingContext)
		{
			if (bindingContext == null)
			{
				throw new ArgumentNullException(nameof(bindingContext));
			}
			ValueProviderResult valueResult =
				bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
			if (valueResult != ValueProviderResult.None && !string.IsNullOrWhiteSpace(valueResult.FirstValue))
			{
				decimal parsedValue = 0m;
				bool binderSucceded = false;
				try
				{
					string formDecValue = valueResult.FirstValue;
					formDecValue = formDecValue.Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
					formDecValue = formDecValue.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
					parsedValue = Convert.ToDecimal(formDecValue);
					binderSucceded = true;
				}
				catch (FormatException fe)
				{
					bindingContext.ModelState.AddModelError(bindingContext.ModelName, fe, bindingContext.ModelMetadata);
				}
				if (binderSucceded)
				{
					bindingContext.Result = ModelBindingResult.Success(parsedValue);
				}
			}
			return Task.CompletedTask;
		}
	}

}
