using System;

namespace company.domain
{
  public class EmployeeFactory : IFactory<Employee>
  {
    private IMediator<IList<string>> _mediator;
  
    public EmployeeFactory(IMediator<IList<string> mediator)
    {
      this._mediator = mediator;
    }
    
    public Employee Create(string name)
    {
      IList<string> names = this._mediator.GetState();
      if(names.Contains(name))
        return new Employee(name);
      return null;
    }
  }
}
