namespace OrderManagement.Application.DTOs;

public record CreateOrderItemDto(int ProductId, int Quantity);
public record CreateOrderDto(int CustomerId, List<CreateOrderItemDto> Items);
public record StockRequestDto(int ProductId, int Quantity, string Reference);
public record CreateCustomerDto(string Name, string Email, string Phone);
public record CreateProductDto(string Name, decimal Price, int ReorderLevel);
public record CreatePaymentDto(int OrderId, decimal Amount, string Method);