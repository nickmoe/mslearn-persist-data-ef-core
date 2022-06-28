using ContosoPizza.Models;
using ContosoPizza.Data;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Services;

public class PizzaService
{
    private readonly PizzaContext _pizzaContext;

    public PizzaService(PizzaContext pizzaContext)
    {
        _pizzaContext = pizzaContext;
    }

    public IEnumerable<Pizza> GetAll()
    {
        return _pizzaContext.Pizzas.AsNoTracking().ToList();
    }

    public Pizza? GetById(int id)
    {
        return _pizzaContext.Pizzas
        .Include(p => p.Sauce)
        .Include(p => p.Toppings)
        .AsNoTracking().SingleOrDefault(p => p.Id == id);
    }

    public Pizza? Create(Pizza newPizza)
    {
        _pizzaContext.Pizzas.Add(newPizza);
        _pizzaContext.SaveChanges();
        return newPizza;
    }

    public void AddTopping(int PizzaId, int ToppingId)
    {
        var pizza = _pizzaContext.Pizzas.Find(PizzaId);
        var topping = _pizzaContext.Toppings.Find(ToppingId);

        if (pizza is null || topping is null)
        {
            throw new InvalidOperationException("Pizza or topping doesn't exist");
        }

        if (pizza.Toppings is null)
        {
            pizza.Toppings = new List<Topping>();
        }

        pizza.Toppings.Add(topping);

        _pizzaContext.SaveChanges();
    }

    public void UpdateSauce(int PizzaId, int SauceId)
    {
        var pizza = _pizzaContext.Pizzas.Find(PizzaId);
        var sauce = _pizzaContext.Sauces.Find(SauceId);

        if (pizza is null || sauce is null)
        {
            throw new InvalidOperationException("Pizza or sauce doesn't exist");
        }

        pizza.Sauce = sauce;
        _pizzaContext.SaveChanges();
    }

    public void DeleteById(int id)
    {
        var pizza = _pizzaContext.Pizzas.Find(id);
        if (pizza is not null)
        {
            _pizzaContext.Pizzas.Remove(pizza);
            _pizzaContext.SaveChanges();
        }
    }
}
