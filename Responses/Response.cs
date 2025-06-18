using System;
using System.Net;

namespace ApiMovie.Responses;

public class Response
{
    public Response()
    {
        Status = (int)HttpStatusCode.InternalServerError;
        Message = HttpStatusCode.InternalServerError.ToString();
    }

    public int Status { get; set; }

    public string Message { get; set; }

    public void Ok()
    {
        Status = (int)HttpStatusCode.OK;
        Message = HttpStatusCode.OK.ToString();
    }

    public void BadRequest()
    {
        Status = (int)HttpStatusCode.BadRequest;
        Message = HttpStatusCode.BadRequest.ToString();
    }

}

public class Response<TData> : Response where TData : class
{
    public TData? Data { get; set; }

    public void Ok(TData? data = null)
    {
        Status = (int)HttpStatusCode.OK;
        Message = HttpStatusCode.OK.ToString();
        if (data != null)
        {
            Data = data;
        }
    }

    public void BadRequest(TData? data = null)
    {
        Status = (int)HttpStatusCode.BadRequest;
        Message = HttpStatusCode.BadRequest.ToString();
        if (data != null)
        {
            Data = data;
        }
    }
}

public class ResponseList<TData> : Response where TData : class
{
    public IEnumerable<TData>? Data { get; set; } = [];

    public PaginationResponseWrapper? Pagination { get; set; } = new PaginationResponseWrapper();

    public void Ok(IEnumerable<TData>? data = null)
    {
        Status = (int)HttpStatusCode.OK;
        Message = HttpStatusCode.OK.ToString();
        if (data != null)
        {
            Data = data;
        }
    }

    public void BadRequest(IEnumerable<TData>? data = null)
    {
        Status = (int)HttpStatusCode.BadRequest;
        Message = HttpStatusCode.BadRequest.ToString();
        if (data != null)
        {
            Data = data;
        }
    }
}

public class PaginationResponseWrapper
{
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalData { get; set; }
    public int TotalPages => PageSize == 0 ? 1 : TotalData / PageSize + 1;
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;
}