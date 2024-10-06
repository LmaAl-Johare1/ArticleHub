import axios from 'axios';

export default function saveBtnClicked(formdata){
    const headers={
        "Content-Type":"application/json",
      }
    const url="http://localhost:50001/api"
    axios.post(url,formdata,{
      headers:headers
    })
    .then((response)=>{
      console.log(response.data)


    }).catch((error)=>{
    const message=error.response.data.message
    
    }
    ) 

     


}
