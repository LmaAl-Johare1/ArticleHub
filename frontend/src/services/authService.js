function loginBtnClicked()

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
           showAlert("Logged in successfully","success")
           

        }).catch((error)=>{
          const message=error.response.data.message
          showAlert(message,"danger")
          }
          )
         
    }




    function registerBtnClicked(){
        const fname = document.getElementById("register-fname-input").value
        const lname = document.getElementById("register-lname-input").value
        const username = document.getElementById("register-username-input").value
        const email = document.getElementById("register-email-input").value
        const password = document.getElementById("register-password-input").value
        

        
       
       

        formData=new FormData()
        formData.append("fname",fname)
        formData.append("lname",lname)
        formData.append("username",username)
        formData.append("email",email)
        formData.append("password",password)
        

        const headers={
            "Content-Type":"multipart/form-data",
          }
        const url=`${baseUrl}/register`
        axios.post(url,formData,{
          headers:headers
        })
        .then((response)=>{
          console.log(response.data)
           localStorage.setItem("token",response.data.token)
           localStorage.setItem("user",JSON.stringify(response.data.user))
           showAlert("New User Registerd Successfully","success")
           

        }).catch((error)=>{
        const message=error.response.data.message
        showAlert(message,"danger")
        }
        ) 

         

   
  }
