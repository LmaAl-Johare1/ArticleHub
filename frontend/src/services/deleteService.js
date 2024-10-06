// import axios from 'axios';
// const baseUrl=""

// function deleteArticleBtnClicked(articleObj)
  
//       {
//         let article =JSON.parse(decodeURIComponent(articleObj))
//         document.getElementById("delete-article-id-input").value=article.id
       
  
  
//       }
  
//       function confirmArticleDelete(){
//           const token=localStorage.getItem("token")
//           const articleId=document.getElementById("delete-article-id-input").value
//           const url=`${baseUrl}/article/${articleId}`
//           const headers={
//               "Content-Type":"multipart/form-data",
//               "authorization":`Bearer ${token}`
//             }
//           axios.delete(url,{
//             headers:headers
//           })
//           .then((response)=>{
             
//              const modal = document.getElementById("delete-article-modal")

  
//           }).catch((error) => {
//             const message= error.response.data.message

  
//           })
//         }