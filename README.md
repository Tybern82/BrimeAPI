# BrimeAPI
Brime v1 Public API

Basic C# Implementation of the Public API v1 for Brime (beta.brimelive.com) as per the current specification at: 
https://documenter.getpostman.com/view/11546462/TzCHBAQ8#7eff8291-1a7c-48cb-a778-fd723204bffa

Please note - the public API is still under development and, while I will attempt to keep this API up to date,
there is no guarantee that this will work with the current active implementation. 

Also note - in order to make calls to the API, you will need to set ClientID in BrimeAPI.com.brimelive.api.BrimeAPIRequest<>
using a valid ClientID for the request being made. (Some requests require special permissions on the ClientID to operate.)
