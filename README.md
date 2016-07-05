# Mediator Pattern
The Mediator Pattern is used to handle complex communication between objects. This way the data exchange is decoupled for the sender and receiver.

In a previous project, I used this pattern to exchange data between objects in a one-to-many relationship. The problem occurred when I was trying to send the data manually to each recipient, but really quick I found that this problem needed a more abstract way in order to exchange the data in a proper way.

Check out the following classes:

```cs
// Client Start Class
public class Client...
  private IFactory<Employee> _factory;
  
  public void Main()
  {
    IList<string> names = new List<string>() { "Paul", "Jacob" };
   
    Employee paul = this._factory.Create("Paul"); 
  }
}

// Factory to create Employee Objects
public class EmployeeFactory : IFactory<Employee>...
  public Employee Create(string name)
  {
    return new Employee(name);
  }
}

// Employee Model Class
public class Employee...
  public string Name { get; set; }
  
  public Employee(string name)
  {
    this.Name = name;
  }
}
```

I would like the share the list of names (Paul & Jacob) so only if the names occur in the list, the ```Employee``` object is created. I could expand the ```IFactory<Employee>``` interface with a sort of ```Invoke()``` method.

```cs
// Client Start Class
public class Client...
  private IFactory<Employee> _factory;
  
  public void Main()
  {
    IList<string> names = new List<string>() { "Paul", "Jacob" };
    this._factory.Invoke(names);
    Employee paul = this._factory.Create("Paul"); 
  }
}

// Factory to create Employee Objects
public class EmployeeFactory : IFactory<Employee>...
  private IList<string> _names;
  
  public void Invoke(object list)
  {
    if(typeof(IList<string>).IsInstanceOfType(list))
      this._names = (Ilist<string>)list;
  }

  public Employee Create(string name)
  {
    if(this._names.Contains(name))
      return new Employee(name);
    return null;
  }
}
```

But I hope that you understand that this isn't the right way to solve this problem. I know, I could create a generic interface ```IInvoke<>``` so the ```object``` would be determined at run-time, but then I still make the Factory responsible for receiving data used inside the factory.

The Factory should be responsible to create objects. That should be the ONLY responsibility. The receive functionality should be removed. But how?

The Mediator Pattern.

```cs
// Client Start Class
public class Client...
  private IMediator<IList<string> _mediator;
  private IFactory<Employee> _factory;
  
  public void Main()
  {
    IList<string> names = new List<string>() { "Paul", "Jacob" };
    this._mediator.SetState(names);
    Employee paul = this._factory.Create("Paul"); 
  }
}

// Factory to create Employee Objects
public class EmployeeFactory : IFactory<Employee>...
  private IMediator<IList<string>> _mediator;

  public Employee Create(string name)
  {
    IList<string> names = this._mediator.GetState();
    if(names.Contains(name))
      return new Employee(name);
    return null;
  }
  
  // Mediator to hold a State
  public class NamesMediator : IMediator<Ilist<string>>...
    private IList<string> _names;
  
    public IList<string> GetState()
    {
      return this._names;
    }
    
    private void SetState(IList<string> names)
    {
      this._names = names;
    }
  }
}
```

In this example, the power of the Mediator Pattern isn't maybe that big. But imagine that you have multiple Factories, Mappers, Respositories, Services... that all need a State. It would take enormously lot of work to maintain all those interfaces (factories, mappers, respositories, services...) to share the right state (all with ```Invoke()``` methods).

The Mediator Pattern allows you to share this state over multiple items: all who needs the state just takes the dependency of the Mediator with the right kind of state:

```cs
public class EmployeeFactory : IFactory<Employee>...
  private IMediator<IList<string>> _mediator;
  
  public EmployeeFactory(IMediator<IList<string>> mediator)
  {
    this._mediator = mediator; 
  }
}
```

This way sharing data across objects isn't that hard anymore. An extra modifictation could be validation logic inside the Mediator to determine of the Mediator contains a valid state.

Thank you, Mediator!
