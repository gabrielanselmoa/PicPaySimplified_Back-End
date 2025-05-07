using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using PicPaySimplified.Application.Dtos.Payment;
using PicPaySimplified.Application.Interfaces;
using PicPaySimplified.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace PicPaySimplified.API.Controllers;

[ApiController]
[Route("payments")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get all payments by user Id", Description = "Retrieves all payments where Payer or Payee Id is equals to the provided user's id ")]
    [SwaggerResponse(StatusCodes.Status200OK, "Payments returned successfully", typeof(ResultModel<PaymentResponse>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Retrieves operation failed")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User is not authenticated")]
    public async Task<IActionResult> Get(string id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return Unauthorized("User is not authenticated");
        var response = await _paymentService.Retrieve(id);
        if (response.Data?.Count == 0)
        {
            return BadRequest("Retrieves operation failed");
        }
        return Ok(response);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Creates a payment (transaction)", Description = "Cria um pagamento com o Valor, Pagador e Recebedor, seguindo as regras de negócio especificadas!")]
    [SwaggerResponse(StatusCodes.Status200OK, "Payments returned successfully", typeof(ResultModel<PaymentResponse>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Creates operation failed")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User is not authenticated")]
    public async Task<IActionResult> Post([FromBody] PaymentRequest request)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return Unauthorized("User is not authenticated");
        var response = await _paymentService.Handle(request);
        return Ok(response);
    }
}