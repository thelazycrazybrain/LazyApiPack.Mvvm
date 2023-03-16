# About this pack
Supports MVVM functionality for applications.


# Caption of a view
If you want to support a caption for a view (to show in a tab), use the [MVVM Caption Pattern](#mvvm-caption-pattern)

# Patterns
## MVVM Caption Pattern
To get a [caption of a view](#caption-of-a-view) (for example - to show in a tab), use this pattern.


**Class Layout:**

```csharp
public class MyView : UserControl 
{
    public MyView() {
        DataContext = new MyViewModel();
    }

    public override string ToString() {
        return "MyView";
    }
}

public class MyViewModel : ISupportModel {
    public MyViewModel() {
        Model = new MyModel();
    }

    public string Title { get; } = "My ViewModel Title";
}

public class MyModel  {
    public string Caption { get; } = "My Model Title";
}

public class MyModelDataContext {

}
```

**Call:**

```csharp
string caption = AppLocalizationManager.Instance.GetMvvmCaption(view);
```

**Result:**

- The result of the call and this class layout would be "My Model Title".
- If the Caption property of MyModel would not exist or would return ```null```, the result would be "My ViewModel Title".
- If the Title property of MyViewModel were ```null```, the result would be "MyView", because the method ```ToString()``` is overridden.
- If the method ```ToString()``` was not overridden, the result would be ```null```.


**Pattern:**

The process goes down in the class hierarchy and then checks, if that class inherits either from ISupportModel (Property: Model) or from FrameworkElement (Property: DataContext) looks for the properties "```Caption```" (Public, Not-Static), "```Title```" (Public, Not-Static) and the ```ToString()``` method (**overridden!**).
If the properties are not found or ```null```, the process goes up one step in the hierarchy and looks for that pattern there until it reaches the topmost element (```view```).

**Example:**

![Alt text](MVVM%20Caption%20Pattern%20Example.png)

