import AuthService from "../Services/Auth/AuthService";
import { Link } from "react-router-dom";
import { Formik, Field, Form, ErrorMessage } from "formik";
import * as Yup from "yup";
import { FaArrowCircleLeft } from "react-icons/fa";
import { useState } from "react";

const LoginForm = () => {
  const [loading, setLoading] = useState(false);

  async function onFinish(values) {
    try {
      setLoading(true);
      const response = await AuthService.login(values.email, values.password);

      if (response.token !== null && response.success === true) {
        // toast.success("Login successful!");
        window.location.reload("/home");
      } else {
        // toast.error("Login failed. Please check your credentials.");
      }
    } catch (error) {
      console.error("An error occurred during login:", error);
      // toast.error("An error occurred during login. Please try again later.");
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className="flex flex-col items-center justify-center h-screen dark">
      <div className="w-full max-w-md bg-gray-800 rounded-lg shadow-md p-6">
        <div className="flex flex-row justify-start">
          <Link to="/">
            <FaArrowCircleLeft size={40} color="white" />
          </Link>
          <h2 className="text-2xl pl-[25%] font-bold text-gray-200 mb-4">
            Sign in
          </h2>
        </div>
        <Formik
          initialValues={{
            email: "",
            password: "",
          }}
          validationSchema={Yup.object({
            email: Yup.string()
              .email("Invalid email address")
              .required("Email is required"),
            password: Yup.string()
              .min(6, "Password must be at least 6 characters")
              .required("Password is required"),
          })}
          onSubmit={onFinish}
        >
          <Form className="flex flex-col">
            <div>
              <Field
                type="email"
                name="email"
                placeholder="Email"
                className="bg-gray-700 text-gray-200 border-0 w-[100%] rounded-md p-2 mb-4 focus:bg-gray-600 focus:outline-none focus:ring-1 focus:ring-blue-500 transition ease-in-out duration-150"
              />
              <ErrorMessage
                name="email"
                component="div"
                className="text-red-500"
              />
            </div>

            <div>
              <Field
                type="password"
                name="password"
                placeholder="Password"
                className="bg-gray-700 text-gray-200 border-0 rounded-md w-[100%] p-2 mb-4 focus:bg-gray-600 focus:outline-none focus:ring-1 focus:ring-blue-500 transition ease-in-out duration-150"
              />
              <ErrorMessage
                name="password"
                component="div"
                className="text-red-500"
              />
            </div>

            <p className="text-white mt-4">
              Don&apos;t have an account?{" "}
              <Link
                to="/register"
                className="text-sm text-blue-500 -200 hover:underline mt-4"
              >
                Register
              </Link>
            </p>
            <button
              className="bg-gradient-to-r from-[#E0B1CB] to-[#240046] text-white font-bold py-2 px-4 rounded-md mt-4 hover:bg-indigo-600 hover:to-[#7B2CBF] transition ease-in-out duration-150"
              type="submit"
            >
              {loading ? "Signing in..." : "Sign in"}
            </button>
          </Form>
        </Formik>
      </div>
    </div>
  );
};

export default LoginForm;
