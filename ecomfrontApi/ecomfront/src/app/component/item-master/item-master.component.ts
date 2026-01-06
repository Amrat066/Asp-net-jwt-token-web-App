import { CommonModule } from '@angular/common';
import { Component, ElementRef, OnInit, inject, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Product } from '../../Model/product';
import { ProductService } from '../../Services/product.service';

@Component({
  selector: 'app-item-master',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './item-master.component.html',
  styleUrl: './item-master.component.css'
})
export class ItemMasterComponent implements OnInit {

  constructor(private fb: FormBuilder) { }
  @ViewChild('myModal') modal: ElementRef | undefined;
  productForm: FormGroup = new FormGroup({})

  productList: Product[] = [];
  formValue: any;

  prodService = inject(ProductService);


  ngOnInit(): void {
    this.setFormState();
    this.getProduct();
  }
  OpenModel() {
    const model = document.getElementById('myModal');
    if (model != null) {
      model.style.display = 'block';
    }
  }
  CloseModel() {
    this.setFormState();
    if (this.modal != null) {
      this.modal.nativeElement.style.display = 'none'
    }
  }

  setFormState() {
    this.productForm = this.fb.group({
      id: [0],
      productName: ['', [Validators.required]],
      price: ['', [Validators.required]],
      description: ['', [Validators.required]],
      rating: ['', [Validators.required]],
      status: [false, [Validators.required]]
    })
  }

  onSubmit() {
    console.log(this.productForm.value)
    if (this.productForm.invalid) {
      alert("pls fill all the fields");
      return;
    } else {
      this.formValue = this.productForm.value as Product;
      const isNew = this.formValue.id === 0;
      const request$ = isNew
        ? this.prodService.addProduct(this.formValue)
        : this.prodService.updateProduct(this.formValue);

      request$.subscribe(() => {
        alert(isNew ? "Product Add Successfully" : "Product updated successfully");
        this.productForm.reset();
        this.getProduct();
        this.CloseModel();
      }, (err) => {
        alert(err?.error ?? "Unable to save product. Please try again.");
      })

    }

  }

  getProduct() {
    this.prodService.getAllProduct().subscribe((res) => {
      this.productList = res;
      console.log(this.productList)
    }, (err) => {
      console.error(err);
      alert(err?.error ?? "Unable to load products. Please login again.");
    })
  }
  onDelete(id: number) {
    const isConfirm = confirm("Are you sure want to delete this record ?")
    if (isConfirm) {
      this.prodService.deleteProduct(id).subscribe((res) => {
        alert("Product deleted successfully");
        this.getProduct();
      }, (err) => {
        console.error(err);
        alert(err?.error ?? "Unable to delete product.");
      })
    } else {
      alert("you select No option.")
    }
  }
  onEdit(product: Product) {
    this.productForm.patchValue({
      id: product.id,
      productName: product.productName,
      price: product.price,
      description: product.description,
      rating: product.rating,
      status: product.status
    });
    this.OpenModel();
  }
}
