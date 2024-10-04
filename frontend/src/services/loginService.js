import axios from 'axios';
const baseUrl="http://localhost:50001/api"

export default function loginBtnClicked()

    {
     
        const Email = document.getElementById("Email-input").value
        const password = document.getElementById("password-input").value
        const params={
            "email": Email,
            "password": password
        }
        const url=`${baseUrl}/users/login`
        axios.post(url,params)
        .then((response)=>{

           localStorage.setItem("token",response.data.token)
           localStorage.setItem("user",JSON.stringify(response.data.user))
           console.log(response.data.token)

        }).catch((error)=>{
          const message=error.response.data.message
          
          }
          )
         
    }




