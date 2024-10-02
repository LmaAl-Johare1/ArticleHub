import axios from 'axios';
const baseUrl=""

export default function loginBtnClicked()

    {
     
        const username = document.getElementById("username-input").value
        const password = document.getElementById("password-input").value
        const params={
            "username": username,
            "password": password
        }
        const url=`${baseUrl}/login`
        axios.post(url,params)
        .then((response)=>{

           localStorage.setItem("token",response.data.token)
           localStorage.setItem("user",JSON.stringify(response.data.user))
           

        }).catch((error)=>{
          const message=error.response.data.message
          
          }
          )
         
    }




