import axios from 'axios';
//const baseUrl="http://localhost:50001/api"

export default function registerBtnClicked(){

    const first_name = document.getElementById("register-fname-input").value
    const last_name = document.getElementById("register-lname-input").value
    const username = document.getElementById("register-username-input").value
    const email = document.getElementById("register-email-input").value
    const password = document.getElementById("register-password-input").value
    
    const formData=new FormData()
    formData.append("first_name",first_name)
    formData.append("last_name",last_name)
    formData.append("username",username)
    formData.append("email",email)
    formData.append("password",password)
    

    const headers={
        "Content-Type":"application/json",
      }
    const url="http://localhost:50001/api/users"
    axios.post(url,formData,{
      headers:headers
    })
    .then((response)=>{
      console.log(response.data)
       localStorage.setItem("token",response.data.token)
       localStorage.setItem("user",JSON.stringify(response.data.user))
    //    showAlert("New User Registerd Successfully","success")
       

    }).catch((error)=>{
    const message=error.response.data.message
    // showAlert(message,"danger")
    }
    ) 

     


}
