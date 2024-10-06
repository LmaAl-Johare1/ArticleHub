import axios from 'axios';
export default function saveBtnClicked(formdata){
  const { first_name,last_name,username,email, bio} = formdata;

  console.log(formdata)
    const headers={
      "Authorization" : "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImxtYV9qb2hhcmUiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJsbWFhbGpvaGFyZUBnbWFpbC5jb20iLCJleHAiOjE3MjgyNDM5NTYsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMC8ifQ.jUdgqpr3tyF5Ddij4MPEmtHMrNYrhTSYNI3eUwPufDQ",
        "Content-Type":"application/json",
      }
    const url="http://localhost:50001/api/users/lma_johare"
    axios.put(url,formdata,{
      
      headers:headers
    })
    .then((response)=>{

      console.log(response.data)


    }).catch((error)=>{
    const message=error.response.data.message
    
    }
    ) 

     


}
