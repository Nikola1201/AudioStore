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

    public void AddToCart(ShoppingCartItem item)
    {
        var cartId = GetOrCreateCartId();
        var cart = GetCart(cartId) ?? new List<ShoppingCartItem>();
        cart.Add(item);
        SetCart(cartId, cart);
    }

    public List<ShoppingCartItem>? GetCart(string cartId)
    {
        var cartCookie = _httpContextAccessor.HttpContext.Request.Cookies[$"Cart_{cartId}"];
        return cartCookie != null ? JsonConvert.DeserializeObject<List<ShoppingCartItem>>(cartCookie) : new List<ShoppingCartItem>();
    }

    public void SetCart(string cartId, List<ShoppingCartItem> cart)
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



