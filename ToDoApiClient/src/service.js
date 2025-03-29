// import axios from 'axios';
import axios1 from '../src/axiosConfig';

axios1.defaults.baseURL=process.env.REACT_APP_API_URL

export default {
  getTasks: async () => {
    try{
    const result = await axios1.get(`/item`)  
    return result.data;
    }
    catch(error){throw error}
  },

  addTask: async(name, isComplete)=>{
console.log('addTask', name)
try{
  const result=await axios1.post(`/item`,
  {Name: name,
    IsComplete:isComplete
  }
  )
  console.log("adddd");
    return {result};
}
catch (error) {
  if (error.response && error.response.status === 404) {
    console.error('Endpoint not found:', error.response.data);
} else {
    console.error('Failed to add task:', error);

  
  }
}
  },

  setCompleted: async(id, isComplete)=>{
    console.log('setCompleted', {id, isComplete})
    // const result = await axios1.put(`/item/${id}`,isComplete)
    const result = await axios1.put(`/item/${id}`, { isComplete });

    return {result};
  },

  deleteTask:async(id)=>{
    console.log('deleteTask')
    const result = await axios1.delete(`/items/${id}`)
    return {result}
  }
};
