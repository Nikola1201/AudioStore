using AudioStore.DataAccess;
using AudioStore.Models;
using AudioStore.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ShoppingCartService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string CartIdCookie = "CartId";
    


    public ShoppingCartService(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
    {
        _httpContextAccessor = httpContextAccessor;
       
    }

    public string GetOrCreateCartId()
    {
        if (_httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(CartIdCookie, out string cartId))
        {
            return cartId;
        }

        cartId = Guid.NewGuid().ToString();
        var options = new CookieOptions { Expires = DateTime.Now.AddDays(30), HttpOnly = true, IsEssential = true };
        _httpContextAccessor.HttpContext.Response.Cookies.Append(CartIdCookie, cartId, options);

        return cartId;
    }

    public void AddToCart(ShoppingCart item)
    {
        var cartId = GetOrCreateCartId();
        var cart = GetCart(cartId) ?? new List<ShoppingCart>();
        cart.Add(item);
        SetCart(cartId, cart);
    }

    public List<ShoppingCart>? GetCart(string cartId)
    {
        var cartCookie = _httpContextAccessor.HttpContext.Request.Cookies[$"Cart_{cartId}"];
        return cartCookie != null ? JsonConvert.DeserializeObject<List<ShoppingCart>>(cartCookie) : new List<ShoppingCart>();
    }

    public void SetCart(string cartId, List<ShoppingCart> cart)
    {
        var options = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.Now.AddDays(7),
            IsEssential = true
        };
        var cartJson = JsonConvert.SerializeObject(cart);
        _httpContextAccessor.HttpContext.Response.Cookies.Append($"Cart_{cartId}", cartJson, options);
    }

   

   
}



