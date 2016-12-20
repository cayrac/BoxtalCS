using System;
using System.Net;

namespace Boxtal
{
  ///<summary>Main Exception thrown by any request, contains informations about the http and request status.</summary>
  ///<remarks>This is the only exception sent by the Boxtal library.</remarks>
  public class ApiException: Exception
  {
    ///<summary>A list of error codes for the ApiException to been sent.</summary>
    ///<remarks></remarks>
    public enum ErrorCode
    {
      ///<summary>Access right limitations.</summary>
      Access,
      ///<summary>Incomplete or invalid request.</summary>
      Request,
      ///<summary>Unknown reasons.</summary>
      Unknown,
      ///<summary>An error occured on the client side.</summary>
      Client,
      ///<summary>An error occured on the server side.</summary>
      Server,
      ///<summary>The request didn't reach the server.</summary>
      Network,
      ///<summary>Couldn't parse the server's response.</summary>
      Parsing
    }

    private ErrorCode _code;
    private HttpStatusCode _httpCode;

    ///<summary>The error code of the actual exception.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public ErrorCode Code {get{return _code;}}
    ///<summary>The HTTP status code of the request which caused the exception.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public HttpStatusCode HttpCode {get{return _httpCode;}}

    ///<summary>Create a new ApiException, represent an error during a request using the Boxtal library.</summary>
    ///<remarks></remarks>
    ///<param name="code">Api error code.</param>
    ///<param name="httpCode">HTTP status code associated with the error.</param>
    ///<param name="message">Message describing the error.</param>
    public ApiException(ErrorCode code, HttpStatusCode httpCode, string message)
    : base(message)
    {
      _code = code;
      _httpCode = httpCode;
    }

    ///<summary>Create a new ApiException, represent an error during a request using the Boxtal library.</summary>
    ///<remarks></remarks>
    ///<param name="code">api error code.</param>
    ///<param name="httpCode">HTTP status code associated with the error.</param>
    ///<param name="message">Message describing the error.</param>
    ///<param name="inner">Exception encapsulated.</param>
    public ApiException(ErrorCode code, HttpStatusCode httpCode, string message, Exception inner)
    : base(message, inner)
    {
      _code = code;
      _httpCode = httpCode;
    }
  }
}
